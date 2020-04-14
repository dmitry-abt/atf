using NLog;
using Rtr.Atf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rtr.Atf.Elements
{
    public class TreeViewElement : Element
    {
        protected static string expandCollapseStatePropertyName = "ExpandCollapse.ExpandCollapseState";

        protected internal TreeViewElement(
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
        }

        public IReadOnlyCollection<TreeViewItemElement> GetItems()
        {
            var elements = this.FindAllElements<TreeViewItemElement>(By.ParentRuntimeId, this.RuntimeId);
            return new ReadOnlyCollection<TreeViewItemElement>(elements.ToList());
        }
    }
}
