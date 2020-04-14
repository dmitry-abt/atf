using NLog;
using Rtr.Atf.Core;

namespace Rtr.Atf.Elements
{
    public class ListBoxItemElement : Element
    {
        private readonly IUiItemWrapper itemWrapper;

        protected internal ListBoxItemElement(
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

        public bool IsSelected
        {
            get
            {
                var t = this.itemWrapper.GetPropertyValue(By.IsSelectedProperty);
                return t == "True";
            }
        }

        private string label;

        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(this.label))
                {
                    var textItem = this.FindElement<TextBlockElement>(By.ParentRuntimeId, this.RuntimeId);
                    this.label = textItem.Text;
                }

                return this.label;
            }
        }
    }
}
