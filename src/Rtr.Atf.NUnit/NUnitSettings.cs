using NLog;
using NUnit.Framework;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.NUnit
{
    public class NUnitSettings : ISettings
    {
        private readonly TestParameters testParameters;

        public NUnitSettings(TestParameters testParameters)
        {
            this.testParameters = testParameters ?? throw new ArgumentNullException(nameof(testParameters));
        }

        public string CurrentTestName => TestContext.CurrentContext.Test.Name;

        public string PackagePath => this.testParameters[nameof(this.PackagePath)];

        public string HostPath => this.testParameters[nameof(this.HostPath)];

        public string HostTitle => this.testParameters[nameof(this.HostTitle)];

        public string MachineUrl => this.testParameters[nameof(this.MachineUrl)];

        public string DeviceName => this.testParameters[nameof(this.DeviceName)];

        public string PlatformName => this.testParameters[nameof(this.PlatformName)];

        public string CommunicationMonitorPath => this.testParameters[nameof(this.CommunicationMonitorPath)];

        public string CommunicationMonitorTitle => this.testParameters[nameof(this.CommunicationMonitorTitle)];

        public string CommunicationLogFolderPath => this.testParameters[nameof(this.CommunicationLogFolderPath)];

        public string CommunicationLogFolderPathAlias => this.testParameters[nameof(this.CommunicationLogFolderPathAlias)];

        public string LogParserPath => this.testParameters[nameof(this.LogParserPath)];

        public CommunicationProtocols ProtocolName
        {
            get
            {
                var value = this.testParameters[nameof(this.ProtocolName)];
                if (value.ToUpper() == "F")
                {
                    return CommunicationProtocols.Fieldbus;
                }

                if (value.ToUpper() == "H")
                {
                    return CommunicationProtocols.Hart;
                }

                throw new ArgumentOutOfRangeException($"Failed to map protocol setting. String value in settings - {value}");
            }
        }

        public int ActionDelay => Convert.ToInt32(this.testParameters[nameof(this.ActionDelay)]);

        public int VisualTreeNavigationRetryDelay => Convert.ToInt32(this.testParameters[nameof(this.VisualTreeNavigationRetryDelay)]);

        public int VisualTreeNavigationRetryCount => Convert.ToInt32(this.testParameters[nameof(this.VisualTreeNavigationRetryCount)]);

        public string TestFrameworkName => "Appium";

        public float DefaultFloatTolerance => throw new NotImplementedException();

        public string Locale => throw new NotImplementedException();

        public Host Host
        {
            get
            {
                if (this.HostTitle == "Reference Run-time Environment")
                {
                    return Host.RRTE;
                }
                else if (this.HostTitle == "AMS Device Manager")
                {
                    return Host.AMS;
                }

                throw new ArgumentException(nameof(this.Host), $"Failed to map {nameof(this.HostTitle)} to known start window titles. Actual title - {this.HostTitle}");
            }
        }

        public string AtfLogPath => this.testParameters[nameof(this.AtfLogPath)];

        public string GetByKey(string key) => this.testParameters[key];

        public bool TryGetByKey(string key, out string result)
        {
            var success = false;

            try
            {
                result = this.GetByKey(key);
                success = true;
            }
            catch (Exception e)
            {
                result = default;
            }

            return success;
        }
    }
}
