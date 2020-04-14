using NLog;
using System;
using System.Collections.Generic;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service for helping locating UI items in a visual tree
    /// and initializing instances of <see cref="Element"/> for located items.
    /// </summary>
    public interface IElementFactory
    {
        /// <summary>
        /// Provides default conditions for locating UI items of given type in a visual tree.
        /// </summary>
        /// <param name="type">A type of element desired to find.</param>
        /// <returns>A enumeration of conditions for locating UI items of desired type.</returns>
        IEnumerable<(string key, object value)> GetDefaultSearchConditions(Type type);

        /// <summary>
        /// Creates an instance of <see cref="Element"/> for UI item.
        /// </summary>
        /// <typeparam name="T">A desired type or subtype of instance to initialize.</typeparam>
        /// <param name="itemWrapper">An automation framework-specific wrapper of UI item.</param>
        /// <param name="uiNavigationProvider">
        /// Service for locating UI items and navigating
        /// among several running applications.
        /// </param>
        /// <param name="elementFactory">
        /// Service for locating UI items in a visual tree
        /// and initializing instances of <see cref="Element"/> for located items.
        /// </param>
        /// <param name="awaitingService">Service for awaiting various conditions to match.</param>
        /// <param name="logger">Logging service.</param>
        /// <returns>Created instance.</returns>
        T CreateElement<T>(
            IUiItemWrapper itemWrapper,
            IUiNavigationProvider uiNavigationProvider,
            IElementFactory elementFactory,
            IAwaitingService awaitingService,
            ILogger logger)
            where T : Element;
    }
}
