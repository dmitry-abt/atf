using NLog;
using Rtr.Atf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rtr.Atf.Elements
{
    public class DataGridRowElement : Element
    {
        private readonly ILogger logger;

        internal DataGridRowElement(
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

        public ReadOnlyCollection<DataGridCellElement> GetCells()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(DataGridRowElement)}: Get cells");

            var elements = this.FindAllElements<DataGridCellElement>((By.ClassNameProperty, "DataGridCell"));
            return new ReadOnlyCollection<DataGridCellElement>(elements);
        }
    }
}
