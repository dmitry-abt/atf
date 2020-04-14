namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides environment and other specific configuration settings.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets current test name.
        /// </summary>
        string CurrentTestName { get; }

        /// <summary>
        /// Gets a path to FDI package. Designed to be relative to the machine
        /// where tests are executed.
        /// </summary>
        string PackagePath { get; }

        /// <summary>
        /// Gets a path to host application. Designed to be relative to the machine
        /// where tests are executed.
        /// </summary>
        string HostPath { get; }

        /// <summary>
        /// Gets a host's window title.
        /// </summary>
        string HostTitle { get; }

        /// <summary>
        /// Gets a current host among known values.
        /// </summary>
        Host Host { get; }

        /// <summary>
        /// Gets a network machine url. used by Appium framework.
        /// </summary>
        string MachineUrl { get; }

        /// <summary>
        /// Gets a machine name where tests are executed.
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// Gets an operating system name on the machine, where tests are executed.
        /// </summary>
        string PlatformName { get; }

        /// <summary>
        /// Gets a path to communication monitoring tool between <see cref="Host"/> and
        /// device. Designed to be relative to the machine where tests are executed.
        /// </summary>
        string CommunicationMonitorPath { get; }

        /// <summary>
        /// Gets a communication monitoring tool's window title.
        /// </summary>
        string CommunicationMonitorTitle { get; }

        /// <summary>
        /// Gets defined protocol, should reflect protocol used on read device.
        /// </summary>
        CommunicationProtocols ProtocolName { get; }

        /// <summary>
        /// Gets a path to a folder for storing communication logs. Designed to be relative
        /// to the machine where tests are executed.
        /// </summary>
        string CommunicationLogFolderPath { get; }

        /// <summary>
        /// Gets a path to a folder for storing communication logs. Designed to be relative
        /// to the machine where test session is handled.
        /// </summary>
        string CommunicationLogFolderPathAlias { get; }

        /// <summary>
        /// Gets a path to a service for parsing logs, received by <see cref="CommunicationMonitorPath"/>.
        /// Designed to be relative to the machine where tests are executed.
        /// </summary>
        string LogParserPath { get; }

        /// <summary>
        /// Gets time in milliseconds to wait between different input actions.
        /// </summary>
        int ActionDelay { get; }

        /// <summary>
        /// Gets time in milliseconds to wait before retry after finding element attempt failed.
        /// </summary>
        int VisualTreeNavigationRetryDelay { get; }

        /// <summary>
        /// Gets amount of attempts to find element or elements in visual tree.
        /// </summary>
        int VisualTreeNavigationRetryCount { get; }

        string AtfLogPath { get; }

        /// <summary>
        /// Gets a name of current test framework.
        /// </summary>
        string TestFrameworkName { get; }

        /// <summary>
        /// Gets default tolerance for comparing float values during communication validation.
        /// </summary>
        float DefaultFloatTolerance { get; }

        /// <summary>
        /// Gets a culture name on the machine where tests are executed.
        /// </summary>
        string Locale { get; }

        /// <summary>
        /// Searches value by key in provided environment settings.
        /// </summary>
        /// <param name="key">A key to search by.</param>
        /// <returns>A value found by key.</returns>
        string GetByKey(string key);

        /// <summary>
        /// Searches value by key in provided environment settings.
        /// </summary>
        /// <param name="key">A key to search by.</param>
        /// <param name="result">When this method returns, contains string representation of value, if searching succeeded,
        /// or default value, if orepation failed.</param>
        /// <returns>A return value indicates whether operation was successful or not.</returns>
        bool TryGetByKey(string key, out string result);
    }
}