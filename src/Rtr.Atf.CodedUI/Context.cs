using NLog;
using Rtr.Atf.Core;
using Rtr.Atf.Elements;
using System.Collections.Generic;
using WindowsInput;

namespace Rtr.Atf.CodedUI
{
    public static class Context
    {
        public static ITestContext Create(
            ISettings settings,
            IEnumerable<IElementFactory> elementFactories)
        {
            var inputSimulator = new InputSimulator();

            var logger = new LoggerFactory().GetLogger(settings);

            var uiNavigationProvider = new CodedUiUiNavigationProvider(
                inputSimulator.Mouse,
                inputSimulator.Keyboard,
                settings,
                logger);

            var elFactories = new List<IElementFactory>()
            {
                new ElementFactory(settings),
            };

            elFactories.AddRange(elementFactories);

            var elementCreator = new ElementCreator(elFactories);

            var sessionHandler = new CodedUiSessionHandler(uiNavigationProvider, settings);

            var navigationService = new NavigationService(
                uiNavigationProvider,
                elementCreator,
                logger);

            var commService = new CommunicationFactory().GetCommunicationService(navigationService, settings, logger);

            var context = new CodedUiTestContext(
                navigationService,
                sessionHandler,
                commService,
                settings,
                logger);

            return context;
        }
    }
}
