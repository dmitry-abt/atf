using NLog;
using Rtr.Atf.Core;
using Rtr.Atf.Elements;
using System.Collections.Generic;

namespace Rtr.Atf.Appium
{
    public static class Context
    {
        public static ITestContext Create(
            ISettings settings,
            IEnumerable<IElementFactory> elementFactories)
        {
            var logger = new LoggerFactory().GetLogger(settings);

            var sessionHandler = new AppiumSessionHandler(settings, logger);

            var wrapperFactory = new AppiumUiItemWrapperFactory();

            var uiNavigationProvider = new AppiumUiNavigationProvider(sessionHandler, wrapperFactory, logger, settings);

            var elFactories = new List<IElementFactory>()
            {
                new ElementFactory(settings),
            };

            elFactories.AddRange(elementFactories);

            var elementCreator = new ElementCreator(elFactories);

            var navigationService = new NavigationService(
                uiNavigationProvider,
                elementCreator,
                logger);

            var commService = new CommunicationFactory().GetCommunicationService(navigationService, settings, logger);

            var context = new AppiumTestContext(
                navigationService,
                sessionHandler,
                commService,
                settings,
                logger);

            return context;
        }
    }
}
