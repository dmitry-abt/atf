using NLog;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;

namespace Rtr.Atf.Elements.RefHost
{
    public class RefHostElementFactory : IElementFactory
    {
        public T CreateElement<T>(
            IUiItemWrapper itemWrapper,
            IUiNavigationProvider uiNavigationProvider,
            IElementFactory elementFactory,
            IAwaitingService awaitingService,
            ILogger logger)
            where T : Element
        {
            var type = typeof(T);

            if (type == typeof(NumericParameterElement))
            {
                return new NumericParameterElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(StringParameterElement))
            {
                return new StringParameterElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(EnumerationParameterElement))
            {
                return new EnumerationParameterElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(BitEnumerationParameterElement))
            {
                return new BitEnumerationParameterElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(DateTimeParameterElement))
            {
                return new DateTimeParameterElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }

            return default;
        }

        public IEnumerable<(string key, object value)> GetDefaultSearchConditions(Type type)
        {
            if (type == typeof(Element) || type == typeof(WindowRootElement))
            {
                // No default search criteria
            }
            else if (type == typeof(NumericParameterElement))
            {
                yield return (By.NameProperty, "Fdi.Ui.ViewModel.Content.NumericParameterViewModel");
            }
            else if (type == typeof(StringParameterElement))
            {
                yield return (By.NameProperty, "Fdi.Ui.ViewModel.Content.StringParameterViewModel");
            }
            else if (type == typeof(EnumerationParameterElement))
            {
                yield return (By.NameProperty, "Fdi.Ui.ViewModel.Content.EnumerationViewModel");
            }
            else if (type == typeof(BitEnumerationParameterElement))
            {
                yield return (By.NameProperty, "Fdi.Ui.ViewModel.Content.BitEnumerationViewModel");
            }
            else if (type == typeof(DateTimeParameterElement))
            {
                yield return (By.NameProperty, "Fdi.Ui.ViewModel.Content.DateTimeParameterViewModel");
            }

            yield break;
        }
    }
}
