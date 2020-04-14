using Rtr.Atf.Core;

namespace Rtr.Atf.CodedUI
{
    public class CodedUiSessionHandler : ISessionHandler
    {
        public CodedUiSessionHandler(CodedUiUiNavigationProvider navigationProvider, ISettings settings)
        {
            this.navigationProvider = navigationProvider;
            this.settings = settings;
        }

        public IUiNavigationProvider UiNavigationProvider { get; }

        public bool EndSession()
        {
            return true;
        }

        private readonly CodedUiUiNavigationProvider navigationProvider;
        private readonly ISettings settings;

        public bool StartSession()
        {
            return true;
        }
    }
}
