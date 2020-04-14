using System;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for awaiting various conditions to match.
    /// </summary>
    public interface IAwaitingService
    {
        /// <summary>
        /// Await for condition function becomes true.
        /// </summary>
        /// <param name="function">A condition function to await.</param>
        void WaitFor(Func<bool> function);

        /// <summary>
        /// Await for condition function becomes true.
        /// </summary>
        /// <param name="function">A condition function to await.</param>
        /// <param name="maximumTime">Maximum amount of time to await.</param>
        void WaitFor(Func<bool> function, TimeSpan maximumTime);

        /// <summary>
        /// Await for set amount of time.
        /// </summary>
        /// <param name="timeSpan">A time to await.</param>
        void WaitFor(TimeSpan timeSpan);

        /// <summary>
        /// Await for default amount of time between input actions.
        /// </summary>
        void WaitForDefaultActionDelay();

        /// <summary>
        /// Await for default amount of time between search UI item or items attempts.
        /// </summary>
        void WaitForDefaultRetryDelay();
    }
}
