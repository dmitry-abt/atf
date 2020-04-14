using NLog;
using Rtr.Atf.Core;

namespace Rtr.Atf.Elements
{
    public class ButtonElement : Element
    {
        public static string ToggleStatePropertyName = "Toggle.ToggleState";
        private readonly ILogger logger;

        protected internal ButtonElement(
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

        public bool GetIsToggleOn()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(ButtonElement)}: Get is button toggled");
            var value = this.Instance.GetPropertyValue(ToggleStatePropertyName);
            this.logger.Trace($"Value is {value}");
            return value == "1";
        }
    }
}
