using NLog;
using System;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for handling logging session with HART Dashboard 2.0.
    /// </summary>
    internal class HartDashboardCommunicationLogger : ICommunicationLogger
    {
        /// <summary>
        /// A default Windows Dialog internal class name.
        /// </summary>
        private const string DialogClassName = "#32770";

        /// <summary>
        /// Identificator for currently running test.
        /// </summary>
        private readonly string testId;

        /// <summary>
        /// Identificator for current log session.
        /// </summary>
        private readonly string sessionId;

        /// <summary>
        /// A service for navigating and windows management during
        /// test session.
        /// </summary>
        private readonly INavigationService navigationService;

        /// <summary>
        /// Environment and other specific configuration settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// A logging service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HartDashboardCommunicationLogger"/> class.
        /// </summary>
        /// <param name="testId">Identificator for currently running test.</param>
        /// <param name="sessionId">Identificator for current log session.</param>
        /// <param name="navigationService">
        /// A service for navigating and windows management during
        /// test session.
        /// </param>
        /// <param name="settings">Environment and other specific configuration settings.</param>
        /// <param name="logger">A logging service.</param>
        internal HartDashboardCommunicationLogger(string testId, string sessionId, INavigationService navigationService, ISettings settings, ILogger logger)
        {
            this.testId = testId;
            this.sessionId = sessionId;
            this.navigationService = navigationService;
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Gets service for awaiting various conditions to match.
        /// </summary>
        private IAwaitingService AwaitingService => this.navigationService.AwaitingService;

        /// <summary>
        /// Begins associated session.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool BeginLogging()
        {
            this.logger.Trace("COMM HART Dashboard 2.0: Start log session");

            var originalWindow = this.navigationService.CurrentWindowTitle;

            this.navigationService.SwitchToWindow(this.settings.CommunicationMonitorTitle);

            var monitorTab = this.navigationService.GetRoot().FindElement(By.AutomationIdProperty, "tabItem3");
            monitorTab.Click();

            monitorTab.FindElement(By.AutomationIdProperty, "buttonOpen").Click();

            this.navigationService.SwitchToWindow(originalWindow);

            return true;
        }

        /// <summary>
        /// Endы associated session.
        /// </summary>
        /// <param name="log">An object that stores all sniffed communication and provides validation tools.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool EndLogging(out ICommunicationLog log)
        {
            this.logger.Trace("COMM HART Dashboard 2.0: Finish log session");

            // navigate to monitoring application
            this.navigationService.SwitchToWindow(this.settings.CommunicationMonitorTitle);

            var monitorTab = this.navigationService.GetRoot().FindElement(By.AutomationIdProperty, "tabItem3");
            monitorTab.Click();

            // stop monitoring and save log to file
            monitorTab.FindElement(By.AutomationIdProperty, "buttonOpen").Click();
            monitorTab.FindElement(By.AutomationIdProperty, "buttonSave").Click();

            var saveDialog = this.navigationService.GetRoot().FindElement(By.ClassNameProperty, DialogClassName);
            var fileEdit = saveDialog.FindElement((By.ClassNameProperty, "Edit"), (By.NameProperty, "File name:"));
            var saveLogPath = @"\" + $"{this.settings.CurrentTestName}_" + $"{this.testId}" + @"\" + $"{this.sessionId}.csv";

            this.logger.Trace("Save log path on machine under test: {saveLogPath}", this.settings.CommunicationLogFolderPath + saveLogPath);

            fileEdit.SendKeys(this.settings.CommunicationLogFolderPath + saveLogPath);
            fileEdit.Instance.PressModifiedCombo(new Keys[] { Keys.Enter });

            this.AwaitingService.WaitFor(TimeSpan.FromMilliseconds(500));

            log = new CommunicationLog(saveLogPath, this.settings, this.logger);

            // clear log
            monitorTab.FindElement(By.AutomationIdProperty, "buttonClear").Click();

            return true;
        }
    }
}
