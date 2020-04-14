using NLog;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides context for executing test case, Serves as an entry point for all tests
    /// based on ATF. Designed to handle single session per context.
    /// </summary>
    public interface ITestContext
    {
        /// <summary>
        /// Gets settings for current test session.
        /// </summary>
        ISettings Settings { get; }

        /// <summary>
        /// Gets service for awaiting various conditions to match.
        /// </summary>
        IAwaitingService AwaitingService { get; }

        /// <summary>
        /// Gets logging service.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Gets service for sniffing and validating communication between
        /// application under test and device.
        /// </summary>
        ICommunicationService CommunicationService { get; }

        /// <summary>
        /// Starts new test session.
        /// </summary>
        /// <returns>A value indicating whether session was started successfully or not.</returns>
        bool StartSession();

        /// <summary>
        /// Finishes previously started session.
        /// </summary>
        /// <returns>A value indicating whether session was finished successfully or not.</returns>
        bool EndSession();

        /// <summary>
        /// Returns root element of window on which context is currently fixed.
        /// </summary>
        /// <returns>Window's root element.</returns>
        Element GetRoot();

        /// <summary>
        /// Returns root element of window with desired header.
        /// </summary>
        /// <param name="windowName">Window's header (name).</param>
        /// <returns>Window's root element.</returns>
        Element GetRoot(string windowName);
    }
}
