using NLog;
using OpenQA.Selenium.Appium.Windows;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.Appium
{
    public class AppiumUiItemWrapperFactory
    {
        private const string DesktopClassName = "#32769";

        public AppiumUiItemWrapper CreateDesktopWrapper(
            WindowsElement element,
            WindowsDriver<WindowsElement> driver,
            ILogger logger,
            ISettings settings,
            IAwaitingService awaitingService)
        {
            return this.CreateWrapper(element, DesktopClassName, driver, logger, settings, awaitingService, null);
        }

        public AppiumUiItemWrapper CreateWrapper(
            WindowsElement element,
            AppiumUiItemWrapper precursor,
            string windowName = default,
            Action<string> getAppRootFunction = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(precursor));
            }

            if (precursor == null)
            {
                throw new ArgumentNullException(nameof(precursor));
            }

            var window = string.IsNullOrEmpty(windowName) ? precursor.ElementContext.WindowName : windowName;
            var driver = precursor.ElementContext.Driver;
            var logger = precursor.ElementContext.Logger;
            var settings = precursor.ElementContext.Settings;
            var awaitingService = precursor.ElementContext.AwaitingService;
            var appRootFunction = getAppRootFunction ?? precursor.ElementContext.BringIntoViewFunc;
            if (appRootFunction == null)
            {
                throw new ArgumentException("Non-desktop element must have application root access function");
            }

            return this.CreateWrapper(element, window, driver, logger, settings, awaitingService, appRootFunction);
        }

        public AppiumUiItemWrapper CreateWrapper(
            WindowsElement element,
            string windowName,
            WindowsDriver<WindowsElement> driver,
            ILogger logger,
            ISettings settings,
            IAwaitingService awaitingService,
            Action<string> appRootFunction)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (awaitingService == null)
            {
                throw new ArgumentNullException(nameof(awaitingService));
            }

            var context = new AppiumElementContext(windowName, driver, logger, settings, this, awaitingService, appRootFunction);

            // create item wrapper
            var res = new AppiumUiItemWrapper(element, context);

            // return wrapper
            return res;
        }
    }
}
