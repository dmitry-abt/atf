using NLog;
using System.Collections.Generic;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Represents window's root UI item.
    /// </summary>
    public class WindowRootElement : Element
    {
        /// <summary>
        /// A logging service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowRootElement"/> class.
        /// </summary>
        /// <param name="itemWrapper">Wrapped UI item by specific UI automation framework.</param>
        /// <param name="uiNavigationProvider">
        /// A service for locating UI items and navigating
        /// among several running applications.
        /// </param>
        /// <param name="elementFactory">
        /// A service for helping locating UI items in a visual tree
        /// and initializing instances of <see cref="Element"/> for located items.
        /// </param>
        /// <param name="awaitingService">A service for awaiting various conditions to match.</param>
        /// <param name="logger">A loggind service.</param>
        protected internal WindowRootElement(
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

        /// <summary>
        /// Closes a window which current element belongs.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool CloseWindow()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(WindowRootElement)}: Close window");
            if (!string.IsNullOrEmpty(this.Instance.RuntimeId))
            {
                this.Instance.PressModifiedCombo(new List<Keys> { Keys.Alt, Keys.F4 }.ToArray());
            }

            return true;
        }

        /// <summary>
        /// Brings into view a window which current element belongs.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool BringIntoView()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(WindowRootElement)}: Bring into view");
            this.Instance.PressModifiedCombo(new List<Keys> { Keys.Windows, Keys.ArrowUp }.ToArray());
            this.Instance.PressModifiedCombo(new List<Keys> { Keys.Windows, Keys.ArrowUp }.ToArray());
            return true;
        }
    }
}
