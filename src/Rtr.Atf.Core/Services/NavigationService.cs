using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides an implmentation of high level methods for navigating
    /// and windows management during test session.
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// A title of Windows Run dialog for launching applications.
        /// </summary>
        private const string RunAppName = "Run";

        /// <summary>
        /// A symbolic part of (Win+R) combination for launching applications.
        /// </summary>
        private const char RunAppKey = 'r';

        /// <summary>
        /// Amount of navigation attempts during closing windows.
        /// </summary>
        private const int ClosingAppAttemptsCount = 2;

        /// <summary>
        /// A service for locating UI items and navigating among several
        /// running applications.
        /// </summary>
        private readonly IUiNavigationProvider uiNavigationProvider;

        /// <summary>
        /// A service for helping locating UI items in a visual tree
        /// and initializing instances of <see cref="Element"/> for located items.
        /// </summary>
        private readonly IElementFactory elementFactory;

        /// <summary>
        /// A logging service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// A list of windows titles visited during test session.
        /// </summary>
        private readonly List<string> visitedWindowNames = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="uiNavigationProvider">
        /// A service for locating UI items and navigating among several
        /// running applications.
        /// </param>
        /// <param name="elementFactory">
        /// A service for helping locating UI items in a visual tree
        /// and initializing instances of <see cref="Element"/> for located items.
        /// </param>
        /// <param name="logger">A logging service.</param>
        public NavigationService(IUiNavigationProvider uiNavigationProvider, IElementFactory elementFactory, ILogger logger)
        {
            this.uiNavigationProvider = uiNavigationProvider;
            this.elementFactory = elementFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Gets title of a window currently in context.
        /// </summary>
        public string CurrentWindowTitle => this.uiNavigationProvider.CurrentWindowTitle;

        /// <summary>
        /// Gets a service for awaiting various conditions to match.
        /// </summary>
        public IAwaitingService AwaitingService => this.uiNavigationProvider.AwaitingService;

        /// <summary>
        /// Finds root element of window currently being in context.
        /// </summary>
        /// <returns>Window's root element.</returns>
        public WindowRootElement GetRoot()
        {
            return this.GetRoot(this.CurrentWindowTitle);
        }

        /// <summary>
        /// Launches an application, given an appplication path and application title.
        /// </summary>
        /// <param name="appPath">A path to application.</param>
        /// <param name="appTitle">A title of application.</param>
        /// <param name="delayMillisecondsAfterLaunch">
        /// The number of milliseconds for which executing is suspended after
        /// launch application sent.
        /// </param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool LaunchApplication(string appPath, string appTitle, int delayMillisecondsAfterLaunch = 2000)
        {
            this.logger.TraceDelimiter().Info("Launch application {appPath}", appPath);

            var r = this.uiNavigationProvider.GetDesktop();
            var root = new Element(r, this.uiNavigationProvider, this.elementFactory, this.AwaitingService, this.logger);
            try
            {
                root.Instance.PressModifiedKey(Keys.Windows, RunAppKey);

                var runRoot = new Element(this.uiNavigationProvider.GetAppRoot(RunAppName), this.uiNavigationProvider, this.elementFactory, this.AwaitingService, this.logger);
                var openEdit = runRoot.FindElement(By.AutomationIdProperty, "12298").FindElement(By.AutomationIdProperty, "1001");

                openEdit.SendKeys(appPath);

                openEdit.Instance.PressKey(Keys.Enter);

                this.AwaitingService.WaitFor(TimeSpan.FromMilliseconds(delayMillisecondsAfterLaunch));

                // TODO: Temporary fix for HART Dashboard v2.0 that sometimes opens in the background and is not functional after that.
                if (appTitle == @"HART Dashboard v2.0")
                {
                    this.logger.TraceDelimiter().Info("Clicking on HART Dashboard tray icon");
                    this.GetRoot("Desktop 1").FindElement(By.ClassNameProperty, @"Shell_TrayWnd").FindAllElements().FirstOrDefault(e => e.Name != null && (e.Name.Contains(@"Metran.Dashboard.Hart") || e.Name.Contains(appTitle))).Click();
                }

                this.SwitchToWindow(appTitle);

                return true;
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                return false;
            }
        }

        /// <summary>
        /// Closes all applications, launched during test session.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool CloseAllApplications()
        {
            var reversedWindowNames = this.visitedWindowNames.AsEnumerable().Reverse();
            foreach (var title in reversedWindowNames)
            {
                try
                {
                    var windowExists = true;
                    while (windowExists)
                    {
                        var root = this.GetRoot(title, ClosingAppAttemptsCount);
                        var cl = root.ClassName;
                        if (!string.IsNullOrEmpty(cl))
                        {
                            root.PressKey(Keys.Escape);
                            this.CloseApplication(title);
                        }
                        else
                        {
                            windowExists = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    this.logger.Error(e);
                }
            }

            return true;
        }

        /// <summary>
        /// Closes an application, given application title.
        /// </summary>
        /// <param name="appTitle">A title of application to close.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool CloseApplication(string appTitle)
        {
            this.logger.Info("Close application {appTitle}", appTitle);

            return this.GetRoot(appTitle, ClosingAppAttemptsCount).CloseWindow();
        }

        /// <summary>
        /// Moves focus of a context to window with given title.
        /// </summary>
        /// <param name="title">A title of a window.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool SwitchToWindow(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Window title cannot be empty or null");
            }

            if (title == this.CurrentWindowTitle)
            {
                return true;
            }

            this.logger.Info("Switch to window {title}", title);

            if (!this.visitedWindowNames.Contains(title))
            {
                this.logger.Trace("Add title {title} to visited windows list", title);
                this.visitedWindowNames.Add(title);
            }

            this.AwaitingService.WaitFor(TimeSpan.FromMilliseconds(1000));

            this.GetRoot(title).BringIntoView();

            return true;
        }

        /// <summary>
        /// Sets service to initial state before any tests were ran.
        /// </summary>
        public void SetToInitialState()
        {
            this.uiNavigationProvider.SetToInitialState();
            this.visitedWindowNames.Clear();
        }

        /// <summary>
        /// Finds root element of window given a window title.
        /// </summary>
        /// <param name="title">A title of window.</param>
        /// <returns>Window's root element.</returns>
        private WindowRootElement GetRoot(string title)
        {
            return new WindowRootElement(this.uiNavigationProvider.GetAppRoot(title), this.uiNavigationProvider, this.elementFactory, this.AwaitingService, this.logger);
        }

        /// <summary>
        /// Finds root element of window given a window title.
        /// </summary>
        /// <param name="title">A title of window.</param>
        /// <param name="retryAmount">Amount of attempts if failed.</param>
        /// <returns>Window's root element.</returns>
        private WindowRootElement GetRoot(string title, int retryAmount)
        {
            return new WindowRootElement(this.uiNavigationProvider.GetAppRoot(title, retryAmount), this.uiNavigationProvider, this.elementFactory, this.AwaitingService, this.logger);
        }
    }
}
