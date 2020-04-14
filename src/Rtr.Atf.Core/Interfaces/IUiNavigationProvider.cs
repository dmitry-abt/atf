using System.Collections.Generic;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for locating UI items and navigating
    /// among several running applications.
    /// </summary>
    public interface IUiNavigationProvider
    {
        /// <summary>
        /// Gets a service for awaiting various conditions to match.
        /// </summary>
        IAwaitingService AwaitingService { get; }

        /// <summary>
        /// Gets header (name) of window currently captured in framework's focus.
        /// </summary>
        string CurrentWindowTitle { get; }

        /// <summary>
        /// Searches for the first descendant UI item of <paramref name="parent"/>
        /// which meets provided conditions.
        /// </summary>
        /// <param name="parent">An element which needs to be searched among its descendant.</param>
        /// <param name="conditions">Conditions which UI item expected to meet.</param>
        /// <returns>First found UI item.</returns>
        IUiItemWrapper FindFirst(Element parent, IEnumerable<(string key, object value)> conditions);

        /// <summary>
        /// Searches for all descendant UI items of <paramref name="parent"/>
        /// which meet provided conditions.
        /// </summary>
        /// <param name="parent">An element which needs to be searched among its descendant.</param>
        /// <param name="conditions">Conditions which UI items expected to meet.</param>
        /// <returns>Found UI items.</returns>
        IEnumerable<IUiItemWrapper> FindAll(Element parent, IEnumerable<(string key, object value)> conditions);

        /// <summary>
        /// Finds root UI item of the window with specified title.
        /// </summary>
        /// <param name="title">Title of window to search.</param>
        /// <param name="retryAmount">Amount of attempts to find an item.</param>
        /// <returns>Found window UI item.</returns>
        IUiItemWrapper GetAppRoot(string title, int retryAmount);

        /// <summary>
        /// Finds root UI item of the window with specified title.
        /// </summary>
        /// <param name="title">Title of window to search.</param>
        /// <returns>Found window UI item.</returns>
        IUiItemWrapper GetAppRoot(string title);

        /// <summary>
        /// Finds desktop's root UI item.
        /// </summary>
        /// <returns>Found desktop's root UI item.</returns>
        IUiItemWrapper GetDesktop();

        /// <summary>
        /// Sets instance to initial state.
        /// </summary>
        void SetToInitialState();
    }
}
