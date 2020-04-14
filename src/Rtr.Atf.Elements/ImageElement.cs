using NLog;
using Rtr.Atf.Core;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Rtr.Atf.Elements
{
    public class ImageElement : Element
    {
        private readonly ILogger logger;
        private readonly string screenshotTemporaryFolder;

        protected internal ImageElement(
            IUiItemWrapper itemWrapper,
            IUiNavigationProvider uiNavigationProvider,
            IElementFactory elementFactory,
            IAwaitingService awaitingService,
            ILogger logger,
            string screenshotTemporaryFolder = null)
            : base(
                  itemWrapper,
                  uiNavigationProvider,
                  elementFactory,
                  awaitingService,
                  logger)
        {
            this.logger = logger;
            this.screenshotTemporaryFolder = screenshotTemporaryFolder;
        }
    }
}
