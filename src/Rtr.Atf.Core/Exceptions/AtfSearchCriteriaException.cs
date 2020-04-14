namespace Rtr.Atf.Core
{
    /// <summary>
    /// A class used for raisinig errors related to search criteria validation.
    /// </summary>
    public class AtfSearchCriteriaException : AtfException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtfSearchCriteriaException"/> class.
        /// </summary>
        /// <param name="message">A message related to exceptional situation.</param>
        public AtfSearchCriteriaException(string message)
            : base(message)
        {
        }
    }
}
