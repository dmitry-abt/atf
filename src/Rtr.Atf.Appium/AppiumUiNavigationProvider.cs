using NLog;
using OpenQA.Selenium.Appium.Windows;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rtr.Atf.Appium
{
    public class AppiumUiNavigationProvider : IUiNavigationProvider
    {
        private const string DesktopClassName = "#32769";

        private readonly AppiumSessionHandler sessionHandler;
        private readonly AppiumUiItemWrapperFactory wrapperFactory;
        private readonly ILogger logger;
        private readonly ISettings settings;

        public AppiumUiNavigationProvider(
            AppiumSessionHandler sessionHandler,
            AppiumUiItemWrapperFactory wrapperFactory,
            ILogger loggerService,
            ISettings settings)
        {
            this.sessionHandler = sessionHandler;
            this.wrapperFactory = wrapperFactory;
            this.logger = loggerService;
            this.settings = settings;
        }

        private string currentWindowTitle;
        private WindowsElement desktopRoot;
        private WindowsElement lastWindow;

        public IEnumerable<IUiItemWrapper> FindAll(Element parent, IEnumerable<(string key, object value)> conditions)
        {
            var s = Stopwatch.StartNew();

            var instance = parent.Instance as AppiumUiItemWrapper;

            var result = instance.FindAll(conditions);

            s.Stop();

            this.logger.Trace("{provider}: Finished search all elements with criteria: {criteria}. Elapsed time: {time} ms", nameof(AppiumUiNavigationProvider), conditions, s.ElapsedMilliseconds);

            return result;
        }

        public IUiItemWrapper FindFirst(Element parent, IEnumerable<(string key, object value)> conditions)
        {
            var s = Stopwatch.StartNew();

            var instance = parent.Instance as AppiumUiItemWrapper;
            var first = instance.FindFirst(conditions);

            s.Stop();

            this.logger.Trace("{provider}: Finished search first element with criteria: {criteria}. Elapsed time: {time} ms.", nameof(AppiumUiNavigationProvider), conditions, s.ElapsedMilliseconds);

            return first;
        }

        public IUiItemWrapper GetAppRoot(string title)
        {
            return this.GetAppRoot(title, 5);
        }

        public IUiItemWrapper GetAppRoot(string title, int retryAmount)
        {
            var s = Stopwatch.StartNew();

            if (title == DesktopClassName)
            {
                this.logger.TraceDelimiter().Trace($"{nameof(AppiumUiNavigationProvider)}: API call get desktop root");

                var desktopRoot = this.GetDesktopWindowsElement();

                return this.wrapperFactory.CreateDesktopWrapper(desktopRoot, this.sessionHandler.CurrentSession, this.logger, this.settings, this.AwaitingService);
            }

            this.logger.TraceDelimiter().Trace($"{nameof(AppiumUiNavigationProvider)}: Get application's root with title {title}");

            if (string.IsNullOrEmpty(this.currentWindowTitle) || this.currentWindowTitle != title)
            {
                WindowsElement window = null;
                var attemptsCount = 0;

                while (window == null && attemptsCount < retryAmount)
                {
                    attemptsCount++;

                    var desktopRoot = this.GetDesktopWindowsElement();

                    try
                    {
                        window = (WindowsElement)desktopRoot.FindElementByName(title);
                    }
                    catch (Exception e)
                    {
                        this.logger.Error(e, $"{nameof(AppiumUiNavigationProvider)}: Get window with title {title} failed on {attemptsCount} attempt");
                    }
                }

                if (window != null)
                {
                    this.lastWindow = window;
                    this.currentWindowTitle = title;
                }
                else
                {
                    var e = new Exception($"{nameof(AppiumUiNavigationProvider)}: Couldn't find Window with title {title}");
                    this.logger.Error(e);
                    throw e;
                }
            }

            var result = this.wrapperFactory.CreateWrapper(
                this.lastWindow,
                (AppiumUiItemWrapper)this.GetDesktop(),
                this.currentWindowTitle,
                (t) =>
                {
                    if (t != this.currentWindowTitle)
                    {
                        var root = this.GetAppRoot(t);
                        root.PressModifiedCombo(new List<Keys> { Keys.Windows, Keys.ArrowUp }.ToArray());
                        this.AwaitingService.WaitForDefaultActionDelay();
                        root.PressModifiedCombo(new List<Keys> { Keys.Windows, Keys.ArrowUp }.ToArray());
                    }
                });

            s.Stop();

            this.logger.Trace("{provider}: Finished getting application's root with title {title}. Elapsed time - {time}ms", nameof(AppiumUiNavigationProvider), title, s.ElapsedMilliseconds);

            return result;
        }

        private WindowsElement GetDesktopWindowsElement()
        {
            if (this.desktopRoot == null)
            {
                this.desktopRoot = this.sessionHandler.CurrentSession.FindElementByClassName(DesktopClassName);
            }

            return this.desktopRoot;
        }

        public IUiItemWrapper GetDesktop()
        {
            return this.GetAppRoot(DesktopClassName);
        }

        public IUiItemWrapper PingDesktopExplicit()
        {
            this.logger.Trace("Getting desktop root explicit");
            this.desktopRoot = this.sessionHandler.CurrentSession.FindElementByClassName(DesktopClassName);
            return this.wrapperFactory.CreateDesktopWrapper(this.desktopRoot, this.sessionHandler.CurrentSession, this.logger, this.settings, this.AwaitingService);
        }

        public void SetToInitialState()
        {
            this.currentWindowTitle = string.Empty;
            this.desktopRoot = null;
            this.lastWindow = null;
        }

        public string CurrentWindowTitle => this.currentWindowTitle;

        private IAwaitingService awaitingService = null;

        public IAwaitingService AwaitingService
        {
            get
            {
                if (this.awaitingService == null)
                {
                    this.awaitingService = new AwaitingServiceFactory().GetAwaitingService(() => this.PingDesktopExplicit(), this.logger, this.settings);
                }

                return this.awaitingService;
            }
        }
    }
}
