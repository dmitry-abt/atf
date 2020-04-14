namespace Rtr.Atf.Core
{
    /// <summary>
    /// A class used for raisinig errors related to navigation through visual tree.
    /// </summary>
    public class AtfNavigationException : AtfException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtfNavigationException"/> class.
        /// </summary>
        /// <param name="message">A message related to exceptional situation.</param>
        public AtfNavigationException(string message)
            : base(message)
        {
        }
    }
}
