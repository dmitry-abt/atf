using NLog;
using System;
using System.Threading;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Basic implementation of <see cref="IAwaitingService"/> that uses <see cref="System.Threading.Thread"/>
    /// for explicit awaiting.
    /// </summary>
    internal class AwaitingService : IAwaitingService
    {
        /// <summary>
        /// A function for locating desktop root element.
        /// </summary>
        private readonly Func<object> desktopPingFunction;

        /// <summary>
        /// A service for logging activity.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Environment and configuration settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwaitingService"/> class.
        /// </summary>
        /// <param name="desktopPingFunction">
        /// A function for locating desktop root element. Supposed to be used for keeping
        /// connection to server alive.
        /// </param>
        /// <param name="logger">A service for logging activity.</param>
        /// <param name="settings">Environment and configuration settings.</param>
        public AwaitingService(Func<object> desktopPingFunction, ILogger logger, ISettings settings)
        {
            this.desktopPingFunction = desktopPingFunction;
            this.logger = logger;
            this.settings = settings;
        }

        /// <summary>
        /// Gets amount of time in milliseconds to wait between input trancactions.
        /// </summary>
        private int ActionDelay => this.settings.ActionDelay;

        /// <summary>
        /// Gets amount of time in milliseconds to wait after transaction failed and before retry.
        /// </summary>
        private int VisualTreeNavigationRetryDelay => this.settings.VisualTreeNavigationRetryDelay;

        /// <summary>
        /// Await for condition function becomes true.
        /// </summary>
        /// <param name="function">A condition function to await.</param>
        /// <param name="maximumTime">Maximum amount of time to await.</param>
        public void WaitFor(Func<bool> function, TimeSpan maximumTime)
        {
            this.logger.Trace("{awaiter}: Start WaitFor. Function - {function}. Maximum await time - {time}", nameof(AwaitingService), function.Method.Name, maximumTime);
            int counter = 0;

            var endTime = DateTime.Now + maximumTime;

            while (!function() && DateTime.Now < endTime)
            {
                this.logger.Trace("{awaiter}: WaitFor {counter} tick", nameof(AwaitingService), counter);
                counter++;
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Await for condition function becomes true.
        /// </summary>
        /// <param name="function">A condition function to await.</param>
        public void WaitFor(Func<bool> function)
        {
            this.WaitFor(function, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Await for set amount of time.
        /// </summary>
        /// <param name="timeSpan">A time to await.</param>
        public void WaitFor(TimeSpan timeSpan)
        {
            this.logger.Trace("{awaiter}: Start WaitFor. Await time - {time}", nameof(AwaitingService), timeSpan);

            int counter = 0;

            var start = DateTime.Now;
            var finish = start + timeSpan;

            while (DateTime.Now < finish)
            {
                this.logger.Trace("      WaitFor {counter} tick", counter);
                counter++;
                this.desktopPingFunction();
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Await for default amount of time between input actions.
        /// </summary>
        public void WaitForDefaultActionDelay()
        {
            Thread.Sleep(this.ActionDelay);
        }

        /// <summary>
        /// Await for default amount of time between search UI item or items attempts.
        /// </summary>
        public void WaitForDefaultRetryDelay()
        {
            Thread.Sleep(this.VisualTreeNavigationRetryDelay);
        }
    }
}
