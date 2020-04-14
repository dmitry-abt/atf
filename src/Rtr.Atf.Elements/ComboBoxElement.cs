using NLog;
using Rtr.Atf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Rtr.Atf.Elements
{
    public class ComboBoxElement : Element
    {
        private readonly IUiItemWrapper itemWrapper;
        private readonly IAwaitingService awaitingService;
        private readonly ILogger logger;

        protected internal ComboBoxElement(
            IUiItemWrapper itemWrapper,
            IUiNavigationProvider uiNavigationProvider,
            IElementFactory elementFactory,
            IAwaitingService awaitingService,
            ILogger logger)
            : base(
                  itemWrapper,
                  uiNavigationProvider,
                  elementFactory,
                  awaitingService,
                  logger)
        {
            this.itemWrapper = itemWrapper;
            this.awaitingService = awaitingService;
            this.logger = logger;
        }

        /// <summary>
        /// Get current combo box value by opening and closing drop-down list.
        /// </summary>
        /// <returns>Selected drop-down list item.</returns>
        public ListBoxItemElement GetSelectedItem()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(ComboBoxElement)}: Get selected item");

            this.Click();

            this.awaitingService.WaitForDefaultActionDelay();

            var item = this.FindElement<ListBoxItemElement>((By.ParentRuntimeId, this.RuntimeId), (By.IsSelectedProperty, "True"));
            this.itemWrapper.PressKey(Keys.Escape);

            return item;
        }

        /// <summary>
        /// Opens drop-down list and gets all list items.
        /// </summary>
        /// <returns>All found drop-down list items.</returns>
        public ReadOnlyCollection<ListBoxItemElement> GetItems()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(ComboBoxElement)}: Get items");

            this.Click();

            this.awaitingService.WaitForDefaultActionDelay();

            var items = this.FindListBoxItems();
            this.itemWrapper.PressKey(Keys.Escape);

            return new ReadOnlyCollection<ListBoxItemElement>(items.ToList());
        }

        /// <summary>
        /// Set combo box value by selecling drop-down list element by its index.
        /// </summary>
        /// <param name="index">Index of dtop-down list item to select.</param>
        public void SetByIndex(int index)
        {
            this.logger.TraceDelimiter().Trace($"{nameof(ComboBoxElement)}: Select item by index {index}");

            this.WaitForEnabled();

            this.Click();

            Thread.Sleep(100);

            var items = this.FindListBoxItems();

            var item = items.ElementAt(index);
            var scroll = this.GetPopup().FindElement(By.AutomationIdProperty, "VerticalScrollBar");

            if (scroll.IsEnabled)
            {
                var pageDownButton = scroll.FindElement(By.AutomationIdProperty, "PageDown");
                while (item.Instance.GetPropertyValue("IsOffscreen").ToLower() == "true")
                {
                    pageDownButton.Instance.Click(MouseButton.Left);
                    Thread.Sleep(50);
                }
            }

            item.Instance.MouseHover();
            Thread.Sleep(50);
            item.Click();
            Thread.Sleep(100);
        }

        /// <summary>
        /// Set combo box value by selecling drop-down list element by its text.
        /// </summary>
        /// <param name="text">Text of dtop-down list item to select.</param>
        public void SetByText(string text)
        {
            this.logger.TraceDelimiter().Trace($"{nameof(ComboBoxElement)}: Select item by text {text}");

            this.WaitForEnabled();

            this.Click();
            Thread.Sleep(100);
            var items = this.FindListBoxItems();

            foreach (var item in items)
            {
                if (item.Label == text)
                {
                    var scroll = this.GetPopup().FindElement(By.AutomationIdProperty, "VerticalScrollBar");

                    if (scroll.IsEnabled)
                    {
                        var pageDownButton = scroll.FindElement(By.AutomationIdProperty, "PageDown");
                        while (item.Instance.GetPropertyValue("IsOffscreen").ToLower() == "true")
                        {
                            pageDownButton.Instance.Click(MouseButton.Left);
                            Thread.Sleep(50);
                        }
                    }

                    item.Instance.MouseHover();
                    Thread.Sleep(50);
                    item.Click();
                    Thread.Sleep(100);
                    return;
                }
            }
        }

        /// <summary>
        /// Finds list box items in an opened drop-down list.
        /// </summary>
        /// <returns>Found list box items.</returns>
        private ReadOnlyCollection<ListBoxItemElement> FindListBoxItems()
        {
            // TODO: Simplify by using keyboard instead of mouse
            var scroll = this.GetPopup().FindElement(By.AutomationIdProperty, "VerticalScrollBar");
            if (scroll.IsEnabled)
            {
                var automationIds = new string[] { "PageDown", "PageUp" };
                foreach (var automationId in automationIds)
                {
                    var button = scroll.FindElement(By.AutomationIdProperty, automationId);
                    while (button.Dimensions.Height > 0)
                    {
                        button.Instance.Click(MouseButton.Left);
                    }
                }
            }

            return this.FindAllElements<ListBoxItemElement>(By.ParentRuntimeId, this.RuntimeId);
        }
    }
}
