using NLog;
using Rtr.Atf.Core;

namespace Rtr.Atf.Elements
{
    public class DataGridCellElement : Element
    {
        internal DataGridCellElement(
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
    }
}
