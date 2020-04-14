using NLog;
using System;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A factory class for prodiving caller with implementation of <see cref="IAwaitingService"/> interface
    /// and handling its initialization.
    /// </summary>
    public class AwaitingServiceFactory
    {
        /// <summary>
        /// Provides an instance of <see cref="ILogger"/>, given parameters.
        /// </summary>
        /// <param name="desktopFunc">
        /// A function that finds desktop root element. Supposed to be used
        /// for keeping test session alive while awaiting for conditions.
        /// </param>
        /// <param name="logger">A logging service.</param>
        /// <param name="settings">Environment and configuration settings.</param>
        /// <returns>An instance of <see cref="ILogger"/>.</returns>
        public IAwaitingService GetAwaitingService(Func<object> desktopFunc, ILogger logger, ISettings settings)
        {
            return new AwaitingService(desktopFunc, logger, settings);
        }
    }
}
