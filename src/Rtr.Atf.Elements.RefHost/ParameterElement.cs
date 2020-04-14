using NLog;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rtr.Atf.Elements.RefHost
{
    public abstract class ParameterElement : Element
    {
        private readonly IAwaitingService awaitingService;

        protected internal ParameterElement(
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

        public virtual string ParameterLabel
        {
            get
            {
                var t = this.FindElement(By.AutomationIdProperty, "Label");
                return t.Name;
            }
        }

        public virtual Element ValueElement { get; }

        public abstract Element GetValueElement();

        public abstract bool IsParameterReadOnly { get; }

        public virtual ImageElement Icon
        {
            get
            {
                var icon = this.FindElement<ImageElement>(By.AutomationIdProperty, "Icon");
                return icon;
            }
        }

        public void WaitForHideWaitIndicator()
        {
            var icon = this.FindElement<ImageElement>(By.AutomationIdProperty, "Icon");

            this.awaitingService.WaitFor(() =>
            {
                var value = 0f;
                using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".Assets.QuestionMark.png"))
                {
                    value = icon.CompareToImage(s);
                }

                return value > 0.1;
            });
        }

        public string UnitText
        {
            get
            {
                this.WaitForHideWaitIndicator();
                var t = this.FindElement(By.AutomationIdProperty, "Unit");
                return t.Name;
            }
        }
    }
}
