namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for handling associated logging session.
    /// </summary>
    public interface ICommunicationLogger
    {
        /// <summary>
        /// Begins associated session.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool BeginLogging();

        /// <summary>
        /// Endы associated session.
        /// </summary>
        /// <param name="log">An object that stores all sniffed communication and provides validation tools.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool EndLogging(out ICommunicationLog log);
    }
}
