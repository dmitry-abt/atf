using NLog;
using Rtr.Atf.Core;
using System.Collections.Generic;

namespace Rtr.Atf.Elements
{
    public class TreeViewItemElement : TreeViewElement
    {
        private readonly IAwaitingService awaitingService;

        protected internal TreeViewItemElement(
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
            this.awaitingService = awaitingService;
        }

        public TreeViewItemState State
        {
            get
            {
                var value = this.Instance.GetPropertyValue(expandCollapseStatePropertyName);
                switch (value)
                {
                    case "Collapsed":
                        return TreeViewItemState.Collapsed;
                    case "Expanded":
                        return TreeViewItemState.Expanded;
                    case "LeafNode":
                        return TreeViewItemState.Leaf;
                    default:
                        return TreeViewItemState.Undefined;
                }
            }
        }

        public IEnumerable<TreeViewItemElement> TryExpand(out bool hasExpanded)
        {
            var state = this.State;
            if (state == TreeViewItemState.Expanded || state == TreeViewItemState.Leaf)
            {
                hasExpanded = false;
                return this.GetItems();
            }

            var button = this.FindElement<ButtonElement>(By.AutomationIdProperty, "Expander");
            button.Click();

            this.awaitingService.WaitForDefaultActionDelay();

            hasExpanded = this.State == TreeViewItemState.Expanded;
            hasExpanded = true;

            return this.GetItems();
        }

        public void TryCollapse(out bool hasCollasped)
        {
            var state = this.State;
            if (state == TreeViewItemState.Collapsed || state == TreeViewItemState.Leaf)
            {
                hasCollasped = false;
                return;
            }

            var button = this.FindElement<ButtonElement>(By.AutomationIdProperty, "Expander");

            button.Click();

            this.awaitingService.WaitForDefaultActionDelay();

            hasCollasped = this.State == TreeViewItemState.Collapsed;
        }
    }
}
