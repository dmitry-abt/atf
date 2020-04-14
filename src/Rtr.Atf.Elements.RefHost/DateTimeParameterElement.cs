using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Rtr.Atf.Core;

namespace Rtr.Atf.Elements.RefHost
{
    public class DateTimeParameterElement : StringParameterElement
    {
        protected internal DateTimeParameterElement(
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
