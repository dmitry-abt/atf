using NLog;
using Rtr.Atf.Core;
using System;
using System.Reflection;

namespace Rtr.Atf.Elements.RefHost
{
    public class StringParameterElement : ParameterElement
    {
        private readonly IAwaitingService awaitingService;
        private readonly ILogger logger;

        protected internal StringParameterElement(
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
            this.awaitingService = awaitingService;
            this.logger = logger;
        }


        public override Element GetValueElement()
        {
            var t = this.FindElement<TextBoxElement>(By.AutomationIdProperty, "Value");
            return t;
        }

        public override Element ValueElement
        {
            get
            {
                return GetValueElement();
            }
        }

        public string GetValue()
        {
            this.WaitForHideWaitIndicator();
            return ((TextBoxElement)this.ValueElement).Text;
        }

        public override bool IsParameterReadOnly
        {
            get
            {
                this.WaitForHideWaitIndicator();
                return ((TextBoxElement)this.ValueElement).IsReadOnly;
            }
        }

        public void SetValue(string value)
        {
            this.WaitForHideWaitIndicator();

            this.logger.TraceDelimiter().Trace($"{nameof(NumericParameterElement)}: Set value {value}");

            this.WaitForEnabled();

            ((TextBoxElement)this.ValueElement).SetValue(value);
        }

        /// <summary>
        /// Checks that each entered symbol in current text box generates a validation error.
        /// </summary>
        /// <param name="invalidSymbols">String of invalid symbols to check.</param>
        /// <returns>Returns true if each symbol in the input set generates an error in current text box.</returns>
        public bool CheckIncorrectSymbols(string invalidSymbols)
        {
            if (string.IsNullOrEmpty(invalidSymbols))
            {
                return false;
            }

            this.logger.TraceDelimiter().Trace("Check entering disallowed symbols to to the current " + $"{nameof(StringParameterElement)}: string to check: {invalidSymbols}");

            System.IO.Stream alertIcon = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rtr.Atf.Elements.RefHost.Assets.AlertMark.png");

            for (int i = 0; i < invalidSymbols.Length; i++)
            {
                ((TextBoxElement)this.ValueElement).SelectAll().SendKeys(invalidSymbols.Substring(i, 1));
                ((TextBoxElement)this.ValueElement).MoveCaretToEnd();

                if (!this.Icon.IsVisible)
                {
                    return false;
                }

                var compareResult = this.Icon.CompareToImage(alertIcon);

                if (compareResult > 0.05)
                {
                    return false;
                }
            }

            alertIcon.Dispose();

            return true;
        }

        /// <summary>
        /// Checks that entered symbols from input string do not generate an error in current text box.
        /// </summary>
        /// <param name="validSymbols">String of symbols.</param>
        /// <param name="maxCount">Maximum number of symbols we can enter at once.</param>
        /// <returns>Returns true if none of allowed symbols generate a validation error when entered.</returns>
        public bool CheckCorrectSymbols(string validSymbols, int maxCount = 8)
        {
            if (string.IsNullOrEmpty(validSymbols) || maxCount == 0)
            {
                return false;
            }

            System.IO.Stream alertIcon = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rtr.Atf.Elements.RefHost.Assets.AlertMark.png");

            int startPosition = 0;

            this.logger.TraceDelimiter().Trace("Check entering allowed symbols to to the current " + $"{nameof(StringParameterElement)}: string to check: {validSymbols}");

            while (startPosition < validSymbols.Length)
            {
                var subString = startPosition + maxCount > validSymbols.Length ? validSymbols.Substring(startPosition) : validSymbols.Substring(startPosition, maxCount);

                ((TextBoxElement)this.ValueElement).SelectAll().SendKeys(subString);
                ((TextBoxElement)this.ValueElement).MoveCaretToEnd();

                if (this.Icon.IsVisible)
                {
                    var compareResult = this.Icon.CompareToImage(alertIcon);

                    if (compareResult < 0.05)
                    {
                        alertIcon.Dispose();

                        // If our icon match with error icon we should return false.
                        return false;
                    }
                }

                startPosition += maxCount;
            }

            alertIcon.Dispose();
            return true;
        }

        /// <summary>
        /// Check that entering more than max allowed number of symbols generates an error.
        /// </summary>
        /// <param name="maxCount">Maximum quantity of symbols which could be entered in text box.</param>
        /// <returns>True if error is generated properly.</returns>
        public bool CheckInputLength(int maxCount)
        {
            if (maxCount == 0)
            {
                return false;
            }

            this.logger.TraceDelimiter().Trace("Check input length " + $"{nameof(StringParameterElement)}: max length allowed: {maxCount}");

            string validString = new string('A', maxCount);
            System.IO.Stream alertIcon = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rtr.Atf.Elements.RefHost.Assets.AlertMark.png");

            ((TextBoxElement)this.ValueElement).SelectAll().SendKeys(validString);
            ((TextBoxElement)this.ValueElement).MoveCaretToEnd();

            if (!this.Icon.IsVisible)
            {
                return false;
            }

            // First we check that we can enter max number of symbols without an error.
            var compareResult = this.Icon.CompareToImage(alertIcon);
            if (compareResult < 0.05)
            {
                alertIcon.Dispose();

                // Here if icon is matching with alert image -  return with false.
                return false;
            }

            ((TextBoxElement)this.ValueElement).SelectAll().SendKeys(validString + "1");
            ((TextBoxElement)this.ValueElement).MoveCaretToEnd();

            compareResult = this.Icon.CompareToImage(alertIcon);
            alertIcon.Dispose();

            if (compareResult > 0.05)
            {
                return false;
            }

            return true;
        }
    }
}