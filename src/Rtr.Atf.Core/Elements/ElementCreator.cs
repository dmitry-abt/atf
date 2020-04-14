using NLog;
using System;
using System.Collections.Generic;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A wrapper factory that uses given element factories for creating elements.
    /// </summary>
    public class ElementCreator : IElementFactory
    {
        /// <summary>
        /// An enumeration of factories to be used for element creation.
        /// </summary>
        private readonly IEnumerable<IElementFactory> elementFactories;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCreator"/> class.
        /// </summary>
        /// <param name="elementFactories">An enumeration of factories to be used for element creation.</param>
        public ElementCreator(IEnumerable<IElementFactory> elementFactories)
        {
            this.elementFactories = elementFactories;
        }

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
        public T CreateElement<T>(
            IUiItemWrapper itemWrapper,
            IUiNavigationProvider uiNavigationProvider,
            IElementFactory elementFactory,
            IAwaitingService awaitingService,
            ILogger logger)
            where T : Element
        {
            foreach (var factory in this.elementFactories)
            {
                T result = factory.CreateElement<T>(
                    itemWrapper,
                    uiNavigationProvider,
                    elementFactory,
                    awaitingService,
                    logger);

                if (result != default(T))
                {
                    return result;
                }
            }

            throw new ArgumentException($"ElementCreator Couldn't create element of type {typeof(T).FullName}");
        }

        /// <summary>
        /// Provides default conditions for locating UI items of given type in a visual tree.
        /// </summary>
        /// <param name="type">A type of element desired to find.</param>
        /// <returns>A enumeration of conditions for locating UI items of desired type.</returns>
        public IEnumerable<(string key, object value)> GetDefaultSearchConditions(Type type)
        {
            foreach (var factory in this.elementFactories)
            {
                var conditionsFound = false;
                foreach (var c in factory.GetDefaultSearchConditions(type))
                {
                    conditionsFound = true;
                    yield return c;
                }

                if (conditionsFound)
                {
                    yield break;
                }
            }

            yield break;
        }
    }
}
