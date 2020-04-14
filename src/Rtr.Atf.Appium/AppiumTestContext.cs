using NLog;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.Appium
{
    // todo Move to ATF.Core
    public class AppiumTestContext : ITestContext
    {
        private readonly INavigationService navigationService;

        private readonly ISessionHandler sessionHandler;

        public AppiumTestContext(
            INavigationService navigationService,
            ISessionHandler sessionHandler,
            ICommunicationService communicationService,
            ISettings settings,
            ILogger logger)
        {
            this.sessionHandler = sessionHandler ?? throw new ArgumentNullException(nameof(sessionHandler));
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.CommunicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISettings Settings { get; }

        public ILogger Logger { get; }

        public IAwaitingService AwaitingService => this.navigationService.AwaitingService;

        public ICommunicationService CommunicationService { get; }

        public bool StartSession()
        {
            this.Logger.TraceDelimiter().Trace($"{nameof(AppiumTestContext)}: Start session");

            this.navigationService.SetToInitialState();

            var appLaunched = this.sessionHandler.StartSession();
            this.navigationService.LaunchApplication(this.Settings.HostPath, this.Settings.HostTitle, 5000);
            var element = this.GetRoot(this.Settings.HostTitle);
            return appLaunched && element != null;
        }

        public bool EndSession()
        {
            this.Logger.TraceDelimiter().Trace($"{nameof(AppiumTestContext)}: End session");

            var appsClosed = this.navigationService.CloseAllApplications();
            var sessionEnded = this.sessionHandler.EndSession();
            return appsClosed && sessionEnded;
        }

        public Element GetRoot()
        {
            return this.navigationService.GetRoot();
        }

        public Element GetRoot(string windowName)
        {
            if (this.navigationService.SwitchToWindow(windowName))
            {
                return this.GetRoot();
            }

            throw new AtfNavigationException($"Failed to get root of {windowName} window");
        }
    }
}
