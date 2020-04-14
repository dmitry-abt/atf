using NLog;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.CodedUI
{
    // todo Move to ATF.Core
    public class CodedUiTestContext : ITestContext
    {
        private readonly INavigationService navigationService;
        private readonly ISessionHandler sessionHandler;

        public CodedUiTestContext(
            INavigationService navigationService,
            ISessionHandler sessionHandler,
            ICommunicationService communicationService,
            ISettings settings,
            ILogger logger)
        {
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.sessionHandler = sessionHandler ?? throw new ArgumentNullException(nameof(sessionHandler));
            this.CommunicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISettings Settings { get; }

        public IAwaitingService AwaitingService => this.navigationService.AwaitingService;

        public ICommunicationService CommunicationService { get; }

        public ILogger Logger { get; }

        public bool StartSession()
        {
            this.Logger.TraceDelimiter().Trace($"{nameof(CodedUiTestContext)}: Start session");

            var appLaunched = this.sessionHandler.StartSession();

            this.navigationService.LaunchApplication(this.Settings.HostPath, this.Settings.HostTitle, 5000);

            var windowSet = this.SwitchToWindow(this.Settings.HostTitle);

            return appLaunched && windowSet;
        }

        public bool EndSession()
        {
            this.Logger.TraceDelimiter().Trace($"{nameof(CodedUiTestContext)}: End session");

            var appsClosed = this.navigationService.CloseAllApplications();
            var sessionEnded = this.sessionHandler.EndSession();

            return appsClosed && sessionEnded;
        }

        public Element GetRoot()
        {
            return this.navigationService.GetRoot();
        }

        public bool SwitchToWindow(string title)
        {
            return this.navigationService.SwitchToWindow(title);
        }

        public Element GetRoot(string windowName)
        {
            throw new NotImplementedException();
        }
    }
}
