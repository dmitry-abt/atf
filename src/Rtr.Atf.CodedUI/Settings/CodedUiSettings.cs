using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Rtr.Atf.Core;
using System;
using System.Collections;

namespace Rtr.Atf.CodedUI
{

    public class CodedUiSettings : ISettings
    {
        private readonly TestContext context;
        private readonly IDictionary testParameters;

        public CodedUiSettings(TestContext context)
        {
            this.testParameters = context.Properties;
            this.context = context;
        }

        public string PackagePath => this.testParameters[nameof(this.PackagePath)].ToString();

        public string HostPath => this.testParameters[nameof(this.HostPath)].ToString();

        public string HostTitle => this.testParameters[nameof(this.HostTitle)].ToString();

        public string MachineUrl => this.testParameters[nameof(this.MachineUrl)].ToString();

        public string DeviceName => this.testParameters[nameof(this.DeviceName)].ToString();

        public string PlatformName => this.testParameters[nameof(this.PlatformName)].ToString();

        public string CommunicationMonitorPath => this.testParameters[nameof(this.CommunicationMonitorPath)].ToString();

        public string CommunicationMonitorTitle => this.testParameters[nameof(this.CommunicationMonitorTitle)].ToString();

        public string CurrentTestName => this.context.TestName;

        public string CommunicationLogFolderPath => this.testParameters[nameof(this.CommunicationLogFolderPath)].ToString();

        public string CommunicationLogFolderPathAlias => this.testParameters[nameof(this.CommunicationLogFolderPathAlias)].ToString();

        public string LogParserPath => this.testParameters[nameof(this.LogParserPath)].ToString();

        public CommunicationProtocols ProtocolName
        {
            get
            {
                var value = this.testParameters[nameof(this.ProtocolName)].ToString();
                if (value.ToUpper() == "FIELDBUS")
                {
                    return CommunicationProtocols.Fieldbus;
                }

                if (value.ToUpper() == "HART")
                {
                    return CommunicationProtocols.Hart;
                }

                throw new ArgumentOutOfRangeException($"Failed to map protocol setting. String value in settings - {value}");
            }
        }

        public int ActionDelay => Convert.ToInt32(this.testParameters[nameof(this.ActionDelay)].ToString());

        public int VisualTreeNavigationRetryDelay => Convert.ToInt32(this.testParameters[nameof(this.VisualTreeNavigationRetryDelay)].ToString());

        public int VisualTreeNavigationRetryCount => Convert.ToInt32(this.testParameters[nameof(this.VisualTreeNavigationRetryCount)].ToString());

        public string TestFrameworkName => "CodedUI";

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

        public float DefaultFloatTolerance => throw new NotImplementedException();

        public string Locale => throw new NotImplementedException();

        public string AtfLogPath => this.testParameters[nameof(this.AtfLogPath)].ToString();

        public string GetByKey(string key)
        {
            return this.testParameters[key].ToString();
        }

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
