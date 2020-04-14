using NLog;
using OpenQA.Selenium.Appium.Windows;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.Appium
{
    internal class AppiumElementContext : IElementContext
    {
        internal AppiumElementContext(
            string windowName,
            WindowsDriver<WindowsElement> driver,
            ILogger logger,
            ISettings settings,
            AppiumUiItemWrapperFactory wrapperFactory,
            IAwaitingService awaitingService,
            Action<string> appRootFunction)
        {
            this.WindowName = windowName;
            this.Settings = settings;
            this.Driver = driver;
            this.WrapperFactory = wrapperFactory;
            this.Logger = logger;
            this.AwaitingService = awaitingService;
            this.BringIntoViewFunc = appRootFunction;
        }

        public string WindowName { get; }

        public ILogger Logger { get; }

        public IAwaitingService AwaitingService { get; }

        public ISettings Settings { get; }

        public AppiumUiItemWrapperFactory WrapperFactory { get; }

        public WindowsDriver<WindowsElement> Driver { get; }

        public Action<string> BringIntoViewFunc { get; }
    }
}
