using NLog;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rtr.Atf.Elements
{
    public class DataGridElement : Element
    {
        private readonly IUiItemWrapper itemWrapper;
        private readonly IAwaitingService awaitingService;
        private readonly ILogger logger;

        internal DataGridElement(
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
            this.awaitingService = awaitingService;
            this.logger = logger;
        }

        public ReadOnlyCollection<DataGridRowElement> GetRows()
        {
            this.logger.TraceDelimiter().Trace($"{nameof(DataGridElement)}: Get rows");

            if (this.GetRowCount() > 0)
            {
                var elements = this.FindAllElements<DataGridRowElement>();

                return new ReadOnlyCollection<DataGridRowElement>(elements.ToList());
            }

            return new ReadOnlyCollection<DataGridRowElement>(new List<DataGridRowElement>());
        }

        public int GetRowCount()
        {
            this.awaitingService.WaitForDefaultActionDelay();
            
            // Thread.Sleep(100); // Coded UI delay
            var result = this.itemWrapper.GetPropertyValue("Grid.RowCount");
            return Convert.ToInt32(result);
        }
    }
}
