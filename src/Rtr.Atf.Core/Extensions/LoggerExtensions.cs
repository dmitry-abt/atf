using NLog;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Extension methods for <see cref="ILogger"/> interface.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Default content for delimiter string.
        /// </summary>
        private static readonly string Delimiter = "====================================================================================";

        /// <summary>
        /// Writes a message, containing visual delimiter at the Trace level.
        /// </summary>
        /// <param name="logger">A logging interface used for writitng messages.</param>
        /// <returns>A logging interface used for writing delimiter message.</returns>
        public static ILogger TraceDelimiter(this ILogger logger)
        {
            logger.Trace(Delimiter);
            return logger;
        }

        /// <summary>
        /// Writes a message, containing visual delimiter at the Info level.
        /// </summary>
        /// <param name="logger">A logging interface used for writitng messages.</param>
        /// <returns>A logging interface used for writing delimiter message.</returns>
        public static ILogger InfoDelimiter(this ILogger logger)
        {
            logger.Info(Delimiter);
            return logger;
        }

        /// <summary>
        /// Writes a message, containing visual delimiter at the Error level.
        /// </summary>
        /// <param name="logger">A logging interface used for writitng messages.</param>
        /// <returns>A logging interface used for writing delimiter message.</returns>
        public static ILogger ErrorDelimiter(this ILogger logger)
        {
            logger.Error(Delimiter);
            return logger;
        }

        /// <summary>
        /// Writes a message, containing visual delimiter at the Warn level.
        /// </summary>
        /// <param name="logger">A logging interface used for writitng messages.</param>
        /// <returns>A logging interface used for writing delimiter message.</returns>
        public static ILogger WarnDelimiter(this ILogger logger)
        {
            logger.Warn(Delimiter);
            return logger;
        }
    }
}
