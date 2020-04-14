using NLog;
using Rtr.Atf.Core;
using System;
using System.Reflection;

namespace Rtr.Atf.Elements.RefHost
{
    public class EnumerationParameterElement : ParameterElement
    {
        private readonly IAwaitingService awaitingService;
        private readonly ILogger logger;

        protected internal EnumerationParameterElement(
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
            this.logger = logger;
        }

        public override Element GetValueElement()
        {
            var t = this.FindElement<ComboBoxElement>(By.AutomationIdProperty, "Value");
            return t;
        }

        public override Element ValueElement
        {
            get
            {
                return this.GetValueElement();
            }
        }

        public void SetValueByText(string text)
        {
            this.WaitForHideWaitIndicator();
            ((ComboBoxElement)this.ValueElement).SetByText(text);
        }

        public void SetValueByIndex(int index)
        {
            this.WaitForHideWaitIndicator();
            ((ComboBoxElement)this.ValueElement).SetByIndex(index);
        }

        public string GetValue()
        {
            this.WaitForHideWaitIndicator();
            return ((ComboBoxElement)this.ValueElement).GetSelectedItem().Label;
        }

        public override bool IsParameterReadOnly
        {
            get
            {
                this.WaitForHideWaitIndicator();
                return !this.ValueElement.IsEnabled;
            }
        }
    }
}
