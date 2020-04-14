using NLog;
using Rtr.Atf.Core;
using System;
using System.Reflection;

namespace Rtr.Atf.Elements.RefHost
{
    public class NumericParameterElement : ParameterElement
    {
        private readonly IAwaitingService awaitingService;
        private readonly ILogger logger;

        protected internal NumericParameterElement(
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
            var t = this.FindElement<TextBoxElement>(By.AutomationIdProperty, "Value");
            return t;
        }

        public override Element ValueElement
        {
            get
            {
                return this.GetValueElement();
            }
        }

        public void SetValue(string value)
        {
            this.WaitForHideWaitIndicator();

            this.logger.TraceDelimiter().Trace($"{nameof(NumericParameterElement)}: Set value {value}");

            this.WaitForEnabled();

            ((TextBoxElement)this.ValueElement).SetValue(value);
        }

        public void SetValue(float value)
        {
            this.SetValue(value.ToString());
        }

        public float GetValue()
        {
            this.WaitForHideWaitIndicator();
            return Convert.ToSingle(((TextBoxElement)this.ValueElement).Text);
        }

        /// <summary>
        /// Returns value of the current element in string type.
        /// </summary>
        /// <returns>String value of the current element.</returns>
        public string GetStringValue()
        {
            this.WaitForHideWaitIndicator();
            return ((TextBoxElement)this.ValueElement).Text;
        }

        public override bool IsParameterReadOnly
        {
            get
            {
                return ((TextBoxElement)this.ValueElement).IsReadOnly;
            }
        }
    }
}
