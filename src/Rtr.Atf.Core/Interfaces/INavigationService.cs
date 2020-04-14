namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides a set of high level methods for navigating
    /// and windows management during test session.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets title of a window currently in context.
        /// </summary>
        string CurrentWindowTitle { get; }

        /// <summary>
        /// Gets a service for awaiting various conditions to match.
        /// </summary>
        IAwaitingService AwaitingService { get; }

        /// <summary>
        /// Finds root element of window currently being in context.
        /// </summary>
        /// <returns>Window's root element.</returns>
        WindowRootElement GetRoot();

        /// <summary>
        /// Launches an application, given an appplication path and application title.
        /// </summary>
        /// <param name="appPath">A path to application.</param>
        /// <param name="appTitle">A title of application.</param>
        /// <param name="delayMillisecondsAfterLaunch">
        /// The number of milliseconds for which executing is suspended after
        /// launch application sent.
        /// </param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool LaunchApplication(string appPath, string appTitle, int delayMillisecondsAfterLaunch = 2000);

        /// <summary>
        /// Sets service to initial state before any tests were ran.
        /// </summary>
        void SetToInitialState();

        /// <summary>
        /// Closes all applications, launched during test session.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool CloseAllApplications();

        /// <summary>
        /// Closes an application, given application title.
        /// </summary>
        /// <param name="appTitle">A title of application to close.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool CloseApplication(string appTitle);

        /// <summary>
        /// Moves focus of a context to window with given title.
        /// </summary>
        /// <param name="title">A title of a window.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool SwitchToWindow(string title);
    }
}
