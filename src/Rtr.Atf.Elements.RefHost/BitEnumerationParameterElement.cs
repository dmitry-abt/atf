using NLog;
using Rtr.Atf.Core;
using System;
using System.Reflection;

namespace Rtr.Atf.Elements.RefHost
{
    public class BitEnumerationParameterElement : ParameterElement
    {
        private readonly IAwaitingService awaitingService;

        protected internal BitEnumerationParameterElement(
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

        public bool GetIsChecked()
        {
            this.WaitForHideWaitIndicator();
            return ((CheckBoxElement)this.ValueElement).GetIsToggleOn();
        }

        public override Element GetValueElement()
        {
            var t = this.FindElement<CheckBoxElement>(By.AutomationIdProperty, "Value");
            return t;
        }

        public override Element ValueElement
        {
            get
            {
                return this.GetValueElement();
            }
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
