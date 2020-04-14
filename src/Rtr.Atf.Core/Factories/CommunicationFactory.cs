using NLog;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A factory that provides instances of <see cref="ICommunicationService"/> and handles its creation.
    /// </summary>
    public class CommunicationFactory
    {
        /// <summary>
        /// Provides an instance of <see cref="ICommunicationService"/>.
        /// </summary>
        /// <param name="navigationService">A service for navigation and window management.</param>
        /// <param name="settings">Environment and configuration settings.</param>
        /// <param name="logger">A logging service.</param>
        /// <returns>An instance of <see cref="ICommunicationService"/>.</returns>
        public ICommunicationService GetCommunicationService(INavigationService navigationService, ISettings settings, ILogger logger)
        {
            return new CommunicationService(navigationService, settings, logger);
        }
    }
}
