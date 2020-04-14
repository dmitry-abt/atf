using Newtonsoft.Json;
using NLog;
using System.Diagnostics;
using System.IO;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Represents structured communication log and provides
    /// validation functionality for it.
    /// </summary>
    internal class CommunicationLog : ICommunicationLog
    {
        /// <summary>
        /// Path to log file.
        /// </summary>
        private readonly string logPath;

        /// <summary>
        /// Settings for current test session.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Logger service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Validator tool instance that provides methods for
        /// communication validation.
        /// </summary>
        private CommunicationValidator validator;

        /// <summary>
        /// Determines if log file was processed and converted to transaction list or not.
        /// </summary>
        private bool isInitialized = false;

        /// <summary>
        /// Communication transactions.
        /// </summary>
        private CommunicationObject rawCommunication = null;

        /// <summary>
        /// Stores unconverted JSON representation received from log file.
        /// </summary>
        private string json = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationLog"/> class.
        /// </summary>
        /// <param name="logPath">Path to log file.</param>
        /// <param name="settings">Settings for current test session.</param>
        /// <param name="logger">Logging service.</param>
        public CommunicationLog(string logPath, ISettings settings, ILogger logger)
        {
            this.logPath = logPath;
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// Gets communication transactions.
        /// </summary>
        public CommunicationObject RawCommunication
        {
            get
            {
                if (!this.isInitialized)
                {
                    this.rawCommunication = this.ParseLogFile();
                    this.isInitialized = true;
                }

                return this.rawCommunication;
            }
        }

        /// <summary>
        /// Gets validator tool instance that provides methods for
        /// communication validation.
        /// </summary>
        public CommunicationValidator Validate
        {
            get
            {
                if (this.validator == null)
                {
                    this.validator = new CommunicationValidator(this.RawCommunication, this.logger);
                }

                return this.validator;
            }
        }

        /// <summary>
        /// Parses provided communication log file and converts it to list
        /// of communication transactions.
        /// </summary>
        /// <returns>Object that contains communication transactions parsed from log file.</returns>
        private CommunicationObject ParseLogFile()
        {
            string logFileFullPath;
            if (this.settings.TestFrameworkName == "CodedUI")
            {
                logFileFullPath = $@"{this.settings.CommunicationLogFolderPath}\{this.logPath}";
            }
            else
            {
                logFileFullPath = $@"{this.settings.CommunicationLogFolderPathAlias}\{this.logPath}";
            }

            long length = new FileInfo(logFileFullPath).Length;

            var iniDirectory = Path.GetDirectoryName(this.settings.LogParserPath) + @"\Shared";

            var parserPath = this.settings.LogParserPath;
            var parserArgs = $@"--logfile {logFileFullPath} --cfg {this.settings.GetByKey("ParserConfigPath")} --inidir {iniDirectory}";

            this.logger.TraceDelimiter().Trace("Start parser process:");

            this.logger.Trace("Parser path:   " + parserPath);
            this.logger.Trace("log file path: " + logFileFullPath);
            this.logger.Trace("File size:     " + length);
            this.logger.Trace("INI directory: " + iniDirectory);
            this.logger.Trace("Args:          " + parserArgs);

            var start = new ProcessStartInfo
            {
                FileName = parserPath,
                Arguments = parserArgs,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            CommunicationObject jsonObj = null;

            using (var process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    this.json = result;

                    var jsonSettings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error,
                    };

                    jsonObj = JsonConvert.DeserializeObject<CommunicationObject>(this.json, jsonSettings);
                }
            }

            return jsonObj;
        }
    }
}
