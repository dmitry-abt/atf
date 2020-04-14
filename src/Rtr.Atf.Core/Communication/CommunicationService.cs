using NLog;
using System;
using System.IO;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for handling communication sniffing and validating.
    /// </summary>
    internal class CommunicationService : ICommunicationService
    {
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
        /// A logging serivce.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Additional internal identificator for currently running test.
        /// Used during caching logs on disk.
        /// </summary>
        private string testId;

        /// <summary>
        /// Represents the amount of launched sniffing sessions.
        /// </summary>
        private int sessionCounter = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationService"/> class.
        /// </summary>
        /// <param name="navigationService">
        /// A service for navigating and windows management during
        /// test session.
        /// </param>
        /// <param name="settings">Environment and other specific configuration settings.</param>
        /// <param name="logger">A logging service.</param>
        public CommunicationService(INavigationService navigationService, ISettings settings, ILogger logger)
        {
            this.navigationService = navigationService;
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Initialize service before performing logging.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool Initialize()
        {
            var lastWindow = this.navigationService.CurrentWindowTitle;

            // run monitor or anithing
            var serviceInitialized = this.navigationService.LaunchApplication(this.settings.CommunicationMonitorPath, this.settings.CommunicationMonitorTitle);

            // return back to current window
            var returnedToWindow = this.navigationService.SwitchToWindow(lastWindow);

            this.testId = Guid.NewGuid().ToString();

            this.logger.Info("Create test log directory");
            this.logger.Info($"Alias path: {this.settings.CommunicationLogFolderPathAlias + @"\" + this.settings.CurrentTestName + "_" + this.testId}");
            this.logger.Info($"Test machine path: {this.settings.CommunicationLogFolderPath + @"\" + this.settings.CurrentTestName + "_" + this.testId}");

            if (this.settings.TestFrameworkName == "CodedUI")
            {
                this.logger.Info("Coded UI: Create test log directory without alias");
                Directory.CreateDirectory(this.settings.CommunicationLogFolderPath + @"\" + this.settings.CurrentTestName + "_" + this.testId);
            }
            else
            {
                this.logger.Info("Appium: Create test log directory using alias");
                Directory.CreateDirectory(this.settings.CommunicationLogFolderPathAlias + @"\" + this.settings.CurrentTestName + "_" + this.testId);
            }

            this.sessionCounter = 0;

            return serviceInitialized && returnedToWindow;
        }

        /// <summary>
        /// Starts new logging session so all communication between host application
        /// and device keep tracked until logging session is stopped.
        /// </summary>
        /// <returns>An instance which handles recently started logging session.</returns>
        public ICommunicationLogger StartNewLogging()
        {
            var commLogger = new HartDashboardCommunicationLogger(this.testId, this.sessionCounter.ToString("D8"), this.navigationService, this.settings, this.logger);

            this.sessionCounter++;

            if (commLogger.BeginLogging())
            {
                return commLogger;
            }

            throw new AtfCommunicationException("Failed to start logging session");
        }
    }
}
