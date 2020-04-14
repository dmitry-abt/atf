namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service, which provides method for handling test session.
    /// </summary>
    public interface ISessionHandler
    {
        /// <summary>
        /// Starts new session.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool StartSession();

        /// <summary>
        /// Ends current session.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool EndSession();
    }
}
