using NLog;
using Rtr.Atf.Core;

namespace Rtr.Atf.Elements
{
    public class TabItemElement : Element
    {
        protected internal TabItemElement(
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

        private TextBlockElement headerTextBlock = null;

        public string HeaderText
        {
            get
            {
                if (this.headerTextBlock == null)
                {
                    this.headerTextBlock = this.TryFindElement<TextBlockElement>();
                }

                if (this.headerTextBlock != null)
                {
                    return this.headerTextBlock.Name;
                }

                return null;
            }
        }
    }
}
