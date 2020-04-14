using NLog;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.Elements
{
    public class TextBoxElement : Element
    {
        private static string textPropertyName = "Value.Value";

        private static string isReadOnlyPropertyName = "Value.IsReadOnly";

        protected internal TextBoxElement(
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

        public void SetValue(string value)
        {
            this.WaitForEnabled();
            this.SelectAll().SendKeys(value).PressKey(Keys.Tab);
        }

        public string Text
        {
            get
            {
                var value = this.Instance.GetPropertyValue(textPropertyName);
                return value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                var value = this.Instance.GetPropertyValue(isReadOnlyPropertyName);

                if (value == null)
                {
                    throw new NullReferenceException("IsReadOnly property");
                }

                return value == "True";
            }
        }

        public TextBoxElement MoveCaretToLeft()
        {
            this.Instance.MoveCaretToLeft();
            return this;
        }

        public TextBoxElement MoveCaretToRight()
        {
            this.Instance.MoveCaretToRight();
            return this;
        }

        public TextBoxElement MoveCaretToEnd()
        {
            this.Instance.MoveCaretToEnd();
            return this;
        }

        public TextBoxElement MoveCaretToBegin()
        {
            this.Instance.MoveCaretToBegin();
            return this;
        }

        public TextBoxElement SelectAll()
        {
            this.Instance.SelectAllShortcut();
            return this;
        }

        public bool CopyToClipboard()
        {
            this.Instance.CopyToClipboardShortcut();
            return true;
        }

        public bool PasteFromClipboard()
        {
            this.Instance.PasteFromClipboardShortcut();
            return true;
        }
    }
}
