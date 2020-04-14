namespace Rtr.Atf.Core
{
    /// <summary>
    /// A class used for raisinig errors related to communication with device.
    /// </summary>
    public class AtfCommunicationException : AtfException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtfCommunicationException"/> class.
        /// </summary>
        /// <param name="message">A message related to exceptional situation.</param>
        public AtfCommunicationException(string message)
            : base(message)
        {
        }
    }
}
