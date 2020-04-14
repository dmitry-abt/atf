using NLog;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.Appium
{
    public class AppiumSessionHandler : ISessionHandler
    {
        private const string AppCapability = "app";
        private const string DeviceNameCapability = "deviceName";
        private const string PlatformNameCapability = "platformName";
        private const string CommandTimeoutCapability = "newCommandTimeout";

        private readonly ILogger logger;
        private readonly ISettings settings;

        public AppiumSessionHandler(ISettings settings, ILogger loggerService)
        {
            this.logger = loggerService;
            this.settings = settings;
        }

        public bool StartSession()
        {
            var url = this.settings.MachineUrl;
            var deviceName = this.settings.DeviceName;
            var platformName = this.settings.PlatformName;
            var desktopLaunched = true;

            if (this.CurrentSession == null || !this.IsSessionAlive())
            {

                WindowsDriver<WindowsElement> desktop = this.StartAppSession(url, deviceName, platformName, "Root");
                desktopLaunched = desktop != null;

                this.CurrentSession = desktop;
            }

            return desktopLaunched;
        }

        private WindowsDriver<WindowsElement> StartAppSession(string url, string deviceName, string platformName, string path)
        {
            AppiumOptions opt = new AppiumOptions();
            opt.AddAdditionalCapability(AppCapability, path);
            opt.AddAdditionalCapability(DeviceNameCapability, deviceName);
            opt.AddAdditionalCapability(PlatformNameCapability, platformName);
            opt.AddAdditionalCapability("newCommandTimeout", 60);

            var resultUrl = $"{url}/wd/hub";

            this.logger.Info($"{AppCapability} - {path}");
            this.logger.Info($"{DeviceNameCapability} - {this.settings.DeviceName}");
            this.logger.Info($"{PlatformNameCapability} - {this.settings.PlatformName}");
            this.logger.Info($"URL - {resultUrl}");

            var desktopSession = new WindowsDriver<WindowsElement>(new Uri(resultUrl), opt, TimeSpan.FromMinutes(2));
            return desktopSession;
        }

        public bool EndSession()
        {
            this.CurrentSession.CloseApp();
            //this.CurrentSession.Quit();

            return true;
        }

        public bool IsSessionAlive()
        {
            WindowsElement desktop = null;

            if (this.CurrentSession == null)
            {
                return false;
            }

            try
            {
                desktop = this.CurrentSession.FindElementByClassName("#32769");
            }
            catch (OpenQA.Selenium.WebDriverException e)
            {
                return false;
            }

            return desktop != null;
        }

        public WindowsDriver<WindowsElement> CurrentSession { get; private set; }
    }
}
