namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for handling communication sniffing and validating.
    /// </summary>
    public interface ICommunicationService
    {
        /// <summary>
        /// Initialize service before performing logging.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool Initialize();

        /// <summary>
        /// Starts new logging session so all communication between host application
        /// and device keep tracked until logging session is stopped.
        /// </summary>
        /// <returns>An instance which handles recently started logging session.</returns>
        ICommunicationLogger StartNewLogging();
    }
}
