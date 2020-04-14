using NLog;
using Rtr.Atf.Core;

namespace Rtr.Atf.Elements
{
    public class TextBlockElement : Element
    {
        private readonly IUiItemWrapper itemWrapper;

        internal TextBlockElement(
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
        }

        public string Text
        {
            get
            {
                return this.itemWrapper.Name;
            }
        }
    }
}
