using NLog;
using Rtr.Atf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rtr.Atf.Elements
{
    public class TabControlElement : Element
    {
        private readonly ILogger logger;

        protected internal TabControlElement(
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
            this.logger = logger;
        }

        public ReadOnlyCollection<TabItemElement> GetTabItems()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(TabControlElement)}: Get tab items");

            var items = this.FindAllElements<TabItemElement>(By.ParentRuntimeId, this.RuntimeId);
            return new ReadOnlyCollection<TabItemElement>(items.ToList());
        }
    }
}
