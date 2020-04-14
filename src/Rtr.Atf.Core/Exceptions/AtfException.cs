using System;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A base class for exceptional situations retaled to ATF workflow.
    /// </summary>
    public class AtfException : Exception
    {
        /// <summary>
        /// A message related to exceptional situation.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtfException"/> class.
        /// </summary>
        /// <param name="message">A message related to exceptional situation.</param>
        public AtfException(string message)
            : base(message)
        {
            this.message = message;
        }
    }
}
