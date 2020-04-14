using NLog;
using NLog.Config;
using System;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A factory that provides instances of <see cref="ILogger"/> and handles its creation.
    /// </summary>
    public class LoggerFactory
    {
        /// <summary>
        /// Provides an instance of <see cref="ILogger"/> logging service.
        /// </summary>
        /// <returns>Instance of <see cref="ILogger"/>.</returns>
        public ILogger GetLogger(ISettings settings)
        {
            var config = new LoggingConfiguration();

            var fileTarget = new NLog.Targets.FileTarget("f")
            {
                FileName = settings.AtfLogPath,
                Layout = @"${uppercase:${level}} ${longdate} ${message}",
            };

            config.AddTarget(fileTarget);

            var debugTarget = new NLog.Targets.DebuggerTarget("debuglog")
            {
                Layout = @"${uppercase:${level}} ${message}",
            };

            config.AddTarget(debugTarget);

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, debugTarget);

            LogManager.Configuration = config;

            var logger = LogManager.GetCurrentClassLogger();

            logger.Info("============================ Started new log session ==========================");
            logger.Info("Start time: {time}", DateTime.Now);
            return logger;
        }
    }
}
