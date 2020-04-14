using NLog;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using WindowsInput;

namespace Rtr.Atf.CodedUI
{
    public class CodedUiUiNavigationProvider : IUiNavigationProvider
    {
        private const string DesktopClassName = "#32769";

        private readonly IMouseSimulator mouse;
        private readonly IKeyboardSimulator keyboard;
        private readonly ISettings settings;
        private readonly ILogger logger;

        public CodedUiUiNavigationProvider(IMouseSimulator mouseSimulator, IKeyboardSimulator keyboardSimulator, ISettings settings, ILogger logger)
        {
            this.mouse = mouseSimulator;
            this.keyboard = keyboardSimulator;
            this.settings = settings;
            this.logger = logger;
        }

        private string currentWindowTitle = string.Empty;
        private AutomationElement desktopRoot = null;
        private AutomationElement lastWindow = null;

        public IEnumerable<IUiItemWrapper> FindAll(Element parent, IEnumerable<(string key, object value)> conditions)
        {
            var s = Stopwatch.StartNew();

            var (codedUiConditions, customConditions) = this.ConvertToCodedUiCondition(conditions);

            var codedUiAutomationElement = parent.Instance as CodedUiUiItemWrapper;

            if (!customConditions.Any())
            {
                foreach (var item in codedUiAutomationElement.FindAll(TreeScope.Descendants, codedUiConditions, conditions))
                {
                    yield return item;
                }
            }
            else
            {
                var elements = codedUiAutomationElement.FindAll(TreeScope.Descendants, codedUiConditions, conditions);

                foreach (var item in elements)
                {
                    var isOk = true;

                    foreach (var (key, value) in customConditions)
                    {
                        if (key == By.ParentRuntimeId)
                        {
                            var p = new CodedUiUiItemWrapper(((CodedUiUiItemWrapper)item).GetParent(), this.mouse, this.keyboard, this.logger, this.settings, this.AwaitingService);
                            if (p.RuntimeId != (string)value)
                            {
                                isOk = false;
                                break;
                            }
                        }
                    }

                    if (isOk)
                    {
                        yield return item;
                    }
                }
            }

            s.Stop();

            this.logger.Trace("{provider}: Finished search all elements with criteria: {criteria}. Elapsed time: {time} ms", nameof(CodedUiUiNavigationProvider), conditions, s.ElapsedMilliseconds);

            yield break;
        }

        public IUiItemWrapper GetRootExplicit
        {
            get
            {
                var t = new CodedUiUiItemWrapper(AutomationElement.RootElement, this.mouse, this.keyboard, this.logger, this.settings, this.AwaitingService);

                return t.FindFirst(TreeScope.Children, Condition.TrueCondition, Enumerable.Empty<(string key, object value)>());
            }
        }

        public string CurrentWindowTitle => this.currentWindowTitle;

        public IUiItemWrapper FindFirst(Element parent, IEnumerable<(string key, object value)> conditions)
        {
            var s = Stopwatch.StartNew();

            var (codedUiConditions, customConditions) = this.ConvertToCodedUiCondition(conditions);

            var codedUiAutomationElement = parent.Instance as CodedUiUiItemWrapper;

            IUiItemWrapper foundElement = null;

            if (!customConditions.Any())
            {
                foundElement = codedUiAutomationElement.FindFirst(TreeScope.Descendants, codedUiConditions, conditions);
            }
            else
            {
                var elements = codedUiAutomationElement.FindAll(TreeScope.Descendants, codedUiConditions, conditions);
                foreach (var item in elements)
                {
                    var isElementPassed = true;

                    foreach (var (key, value) in customConditions)
                    {
                        if (key == By.ParentRuntimeId)
                        {
                            var p = new CodedUiUiItemWrapper(((CodedUiUiItemWrapper)item).GetParent(), this.mouse, this.keyboard, this.logger, this.settings, this.AwaitingService);
                            this.logger.Trace("Parent runtime id = {id}", p.RuntimeId);
                            if (p.RuntimeId != (string)value)
                            {
                                isElementPassed = false;
                                break;
                            }
                        }
                    }

                    // We search first element passed all conditions
                    if (isElementPassed)
                    {
                        foundElement = item;
                        break;
                    }
                }
            }

            s.Stop();

            if (foundElement != null)
            {
                this.logger.Trace("{provider}: Finished search first element with criteria: {criteria}. Elapsed time: {time} ms.", nameof(CodedUiUiNavigationProvider), conditions, s.ElapsedMilliseconds);
            }
            else
            {
                this.logger.Trace("{provider}: Failed search first element with criteria: {criteria}. Elapsed time: {time} ms.", nameof(CodedUiUiNavigationProvider), conditions, s.ElapsedMilliseconds);
            }

            return foundElement;
        }

        public IUiItemWrapper GetAppRoot(string title)
        {
            this.AwaitingService.WaitFor(TimeSpan.FromSeconds(1));
            var s = Stopwatch.StartNew();

            if (title == DesktopClassName)
            {
                this.logger.Trace($"{nameof(CodedUiUiNavigationProvider)}: Getting desktop root");
                if (this.desktopRoot == null)
                {
                    this.desktopRoot = AutomationElement.RootElement;
                }

                return new CodedUiUiItemWrapper(this.desktopRoot, this.mouse, this.keyboard, this.logger, this.settings, this.AwaitingService);
            }

            this.logger.Trace($"{nameof(CodedUiUiNavigationProvider)}: Get application's root with title {title}");

            if (string.IsNullOrEmpty(this.currentWindowTitle) || this.currentWindowTitle != title)
            {
                AutomationElement window = null;
                var attemptsCount = 0;

                while (window == null && attemptsCount < 10)
                {
                    attemptsCount++;
                    if (this.desktopRoot == null)
                    {
                        this.desktopRoot = AutomationElement.RootElement;
                    }

                    try
                    {
                        window = this.desktopRoot.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, title));
                    }
                    catch (Exception e)
                    {
                        this.logger.Error(e, $"{nameof(CodedUiUiNavigationProvider)}: Get window with title {title} failed on {attemptsCount} attempt");
                        throw e;
                    }
                }

                if (window != null)
                {
                    this.lastWindow = window;
                    this.currentWindowTitle = title;
                }
                else
                {
                    var e = new Exception($"{nameof(CodedUiUiNavigationProvider)}: Couldn't find Window with title {title}");
                    this.logger.Error(e);
                    throw e;
                }
            }

            var result = new CodedUiUiItemWrapper(this.lastWindow, this.mouse, this.keyboard, this.logger, this.settings, this.AwaitingService);

            s.Stop();

            this.logger.Trace("{provider}: Finished getting application's root with title {title}. RuntimeId - {runtimeId}. Elapsed time - {time}ms", nameof(CodedUiUiNavigationProvider), title, result.RuntimeId, s.ElapsedMilliseconds);

            this.lastWindow.SetFocus();

            return result;
        }

        public IUiItemWrapper GetDesktop()
        {
            return this.GetAppRoot(DesktopClassName);
        }

        private (Condition codedUiConditions, IEnumerable<(string key, object value)> customConditions) ConvertToCodedUiCondition(IEnumerable<(string key, object value)> conditions)
        {
            Condition codedUiResult = Condition.TrueCondition;
            var customResult = new List<(string key, object value)>();

            if (conditions == null)
            {
                return (codedUiResult, Enumerable.Empty<(string key, object value)>());
            }

            foreach (var (key, value) in conditions)
            {
                if (key == By.ParentRuntimeId)
                {
                    this.logger.Trace("    Add custom condition ({key}, {value})", key, value);
                    customResult.Add((key, value));
                }
                else
                {
                    this.logger.Trace("    Add Coded UI condition ({key}, {value})", key, value);
                    var condition = new PropertyCondition(PropertyMapper.ToCodedUiAutomationProperty(key), value);
                    codedUiResult = new AndCondition(codedUiResult, condition);
                }
            }

            return (codedUiResult, customResult);
        }

        private IAwaitingService awaitingService = null;

        public IAwaitingService AwaitingService
        {
            get
            {
                if (this.awaitingService == null)
                {
                    this.awaitingService = new AwaitingServiceFactory().GetAwaitingService(() => this.GetRootExplicit, this.logger, this.settings);
                }

                return this.awaitingService;
            }
        }

        public IUiItemWrapper GetAppRoot(string title, int retryAmount)
        {
            throw new NotImplementedException();
        }

        public void SetToInitialState()
        {
            throw new NotImplementedException();
        }
    }
}
