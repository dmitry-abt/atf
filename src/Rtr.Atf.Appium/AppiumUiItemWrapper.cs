using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Rtr.Atf.Appium
{
    public class AppiumUiItemWrapper : IUiItemWrapper
    {
        private readonly string notVisibleBoundingRectangle = "Left:0 Top:0 Width:0 Height:0";

        public static readonly string NamePropertyName = "Name";
        public static readonly string RuntimeIdPropertyName = "RuntimeId";
        public static readonly string IsEnabledPropertyName = "IsEnabled";
        public static readonly string HelpTextPropertyName = "HelpText";
        public static readonly string ClassNamePropertyName = "ClassName";
        public static readonly string ControlTypePropertyName = "ControlType";
        public static readonly string LocalizedControlTypePropertyName = "LocalizedControlType";
        public static readonly string AutomationIdPropertyName = "AutomationId";
        public static readonly string ISelectedPropertyName = "SelectionItem.IsSelected";

        private readonly WindowsElement windowsElement;
        private readonly int defaultVisualTreeNavigationRetryCount;

        internal IElementContext ElementContext { get; }

        private WindowsDriver<WindowsElement> Driver => this.ElementContext.Driver;

        private ILogger Logger => this.ElementContext.Logger;

        private ISettings Settings => this.ElementContext.Settings;

        private IAwaitingService AwaitingService => this.ElementContext.AwaitingService;

        internal AppiumUiItemWrapper(WindowsElement windowsElement, IElementContext elementContext)
            : this(elementContext)
        {
            this.windowsElement = windowsElement ?? throw new ArgumentNullException(nameof(windowsElement));
        }

        internal AppiumUiItemWrapper(IElementContext elementContext)
        {
            this.ElementContext = elementContext ?? throw new ArgumentNullException(nameof(elementContext));

            this.defaultVisualTreeNavigationRetryCount = this.Settings.VisualTreeNavigationRetryCount;
        }

        private void TryBringIntoView()
        {
            this.AwaitingService.WaitForDefaultActionDelay();
            this.ElementContext.BringIntoViewFunc?.Invoke(this.ElementContext.WindowName);
        }

        internal IUiItemWrapper FindFirst(IEnumerable<(string key, object value)> conditions)
        {
            this.Logger.Trace("{wrapper}: Search first element with criteria: {criteria}", nameof(AppiumUiItemWrapper), conditions);

            this.TryBringIntoView();

            var s = Stopwatch.StartNew();

            WindowsElement foundElement = null;
            var attemptsCount = 0;

            while (foundElement == null && attemptsCount < this.defaultVisualTreeNavigationRetryCount)
            {
                attemptsCount++;
                foundElement = this.FindFirstInternal(conditions);
                if (foundElement == null)
                {
                    this.Logger.Trace("Couldn't find element on {attempt} attempt", attemptsCount);
                    this.AwaitingService.WaitForDefaultRetryDelay();
                }
            }

            s.Stop();

            if (foundElement == null)
            {
                this.Logger.Error("{wrapper}: Failed search first element with criteria: {criteria}. Elepsed time: {time}ms", nameof(AppiumUiItemWrapper), conditions, s.ElapsedMilliseconds);
                throw new AtfNavigationException($"Failed search first element with criteria: {conditions}");
            }

            this.Logger.Trace("{wrapper}: Completed search first element with criteria: {criteria}. Elapsed time: {time}ms", nameof(AppiumUiItemWrapper), conditions, s.ElapsedMilliseconds);

            return this.ElementContext.WrapperFactory.CreateWrapper(foundElement, this);
        }

        private WindowsElement FindFirstInternal(IEnumerable<(string key, object value)> conditions)
        {
            WindowsElement foundElement = null;

            try
            {
                if (conditions.HasOne())
                {
                    var (key, value) = conditions.First();
                    foundElement = (WindowsElement)this.FindFirstSingleCondition(key, value);
                }
                else if (!conditions.Any())
                {
                    if (this.windowsElement != null)
                    {
                        foundElement = (WindowsElement)this.windowsElement.FindElement(OpenQA.Selenium.By.XPath("//*"));
                    }
                    else
                    {
                        foundElement = this.Driver.FindElement(OpenQA.Selenium.By.XPath("//*"));
                    }
                }
                else
                {
                    var foundItems = new List<IWebElement>();

                    IEnumerable<(string key, object value)> sortedConditions = this.SortConditions(conditions);

                    var main = sortedConditions.First();
                    var other = sortedConditions.Skip(1);
                    var els = this.FindAllSingleCondition(main.key, main.value).Cast<WindowsElement>();

                    foreach (var el in els)
                    {
                        bool isOk = true;
                        foreach (var (key, value) in other)
                        {
                            if (key == Core.By.AutomationIdProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, AutomationIdPropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.ClassNameProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, ClassNamePropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.HelpTextProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, HelpTextPropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.NameProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, NamePropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.RuntimeIdProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, RuntimeIdPropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.ControlTypeProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, ControlTypePropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.LocalizedControlTypeProperty)
                            {
                                if ((string)value != this.GetPropertyValue(el, LocalizedControlTypePropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                            else if (key == Core.By.IsSelectedProperty)
                            {
                                var v = this.GetPropertyValue(el, ISelectedPropertyName);
                                if ((string)value != this.GetPropertyValue(el, ISelectedPropertyName))
                                {
                                    isOk = false;
                                    break;
                                }
                            }
                        }

                        if (isOk)
                        {
                            foundItems.Add(el);
                            break;
                        }
                    }

                    foundElement = (WindowsElement)foundItems.FirstOrDefault();
                }
            }
            catch (AtfSearchCriteriaException e)
            {
                this.Logger.Error(e);
                throw e;
            }
            catch (Exception e)
            {
                this.Logger.Error(e);
            }

            return foundElement;
        }

        private readonly Dictionary<string, int> priorityDict = new Dictionary<string, int>
        {
            { Core.By.AutomationIdProperty, 100 },
            { Core.By.RuntimeIdProperty, 98 },
            { Core.By.NameProperty, 96 },
            { Core.By.HelpTextProperty, 94 },
            { Core.By.LocalizedControlTypeProperty, 93 },
            { Core.By.ClassNameProperty, 92 },
            { Core.By.IsSelectedProperty, 91 },
            { Core.By.ControlTypeProperty, 90 },
            { Core.By.ParentRuntimeId, 89 },
        };

        private IEnumerable<(string key, object value)> SortConditions(IEnumerable<(string key, object value)> conditions)
        {
            var ps = new List<((string key, object value) condition, int priority)>();
            foreach (var c in conditions)
            {
                ps.Add((c, this.priorityDict[c.key]));
            }

            var result = ps.OrderByDescending(p => p.priority).Select(p => p.condition).ToList();

            return result;
        }

        internal IReadOnlyCollection<IUiItemWrapper> FindAll(IEnumerable<(string, object)> conditions)
        {
            this.Logger.Trace("{wrapper}: Search all elements with criteria: {criteria}", nameof(AppiumUiItemWrapper), conditions);

            this.TryBringIntoView();

            var s = Stopwatch.StartNew();

            IEnumerable<IWebElement> foundItems = null;

            var attemptsCount = 0;

            while ((foundItems == null || !foundItems.Any()) && attemptsCount < this.defaultVisualTreeNavigationRetryCount)
            {
                attemptsCount++;

                foundItems = this.FindAllInternal(conditions);

                if (foundItems == null || !foundItems.Any())
                {
                    this.Logger.Trace("Couldn't find elements on {attempt} attempt", attemptsCount);
                    this.AwaitingService.WaitForDefaultRetryDelay();
                }
            }

            s.Stop();

            this.Logger.Trace("{wrapper}: Completed search all elements with criteria: {criteria}. Elapsed time: {time} ms", nameof(AppiumUiItemWrapper), conditions, s.ElapsedMilliseconds);

            var list = foundItems.Select(w => (IUiItemWrapper)this.ElementContext.WrapperFactory.CreateWrapper((WindowsElement)w, this)).ToList();
            return new ReadOnlyCollection<IUiItemWrapper>(list);
        }

        private IEnumerable<IWebElement> FindAllInternal(IEnumerable<(string, object)> conditions)
        {
            var foundItems = new List<IWebElement>();

            if (!conditions.Any())
            {
                if (this.windowsElement != null)
                {
                    var elements = this.windowsElement.FindElements(OpenQA.Selenium.By.XPath("//*"));
                    foundItems.AddRange(elements);
                }
                else
                {
                    var elements = this.Driver.FindElements(OpenQA.Selenium.By.XPath("//*"));
                    foundItems.AddRange(elements);
                }
            }
            else
            {
                foreach (var (key, value) in conditions)
                {
                    IReadOnlyCollection<IWebElement> elements = null;

                    elements = this.FindAllSingleCondition(key, value);

                    if (elements == null || !elements.Any())
                    {
                        return null;
                    }

                    if (!foundItems.Any())
                    {
                        foundItems.AddRange(elements);
                    }
                    else
                    {
                        foundItems = foundItems.Intersect(elements).ToList();
                        if (!foundItems.Any())
                        {
                            return null;
                        }
                    }
                }
            }

            return foundItems;
        }

        private IReadOnlyCollection<IWebElement> FindAllSingleCondition(string key, object value)
        {
            if (key == Core.By.ClassNameProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementsByClassName((string)value);
                }

                return this.Driver.FindElementsByClassName((string)value);
            }

            if (key == Core.By.ControlTypeProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElements(OpenQA.Selenium.By.XPath($@".//*[@ControlType='{value}']"));
                }

                return this.Driver.FindElements(OpenQA.Selenium.By.XPath($@".//*[@ControlType='{value}']"));
            }

            if (key == Core.By.LocalizedControlTypeProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElements(OpenQA.Selenium.By.XPath($@".//*[@LocalizedControlType='{value}']"));
                }

                return this.Driver.FindElements(OpenQA.Selenium.By.XPath($@".//*[@LocalizedControlType='{value}']"));
            }

            if (key == Core.By.NameProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementsByName((string)value);
                }

                return this.Driver.FindElementsByName((string)value);
            }

            if (key == Core.By.HelpTextProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElements(OpenQA.Selenium.By.XPath($@".//*[@HelpText='{value}']"));
                }

                return this.Driver.FindElements(OpenQA.Selenium.By.XPath($@".//*[@HelpText='{value}']"));
            }

            if (key == Core.By.AutomationIdProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementsByAccessibilityId((string)value);
                }

                return this.Driver.FindElementsByAccessibilityId((string)value);
            }

            if (key == Core.By.ParentRuntimeId)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementsByXPath($".//*[parent::*[@RuntimeId='{value}']]");
                }

                return this.Driver.FindElementsByXPath($".//*[parent::*[@RuntimeId='{value}']]");
            }

            if (key == Core.By.IsSelectedProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementsByXPath($@".//*[@IsSelected='{value}']");
                }

                return this.Driver.FindElementsByXPath($@".//*[@IsSelected='{value}']");
            }

            if (key == "Parent")
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementsByXPath(@"//parent::");
                }
            }

            throw new AtfSearchCriteriaException(key);
        }

        private AppiumWebElement FindFirstSingleCondition(string key, object value)
        {
            if (key == Core.By.ClassNameProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementByClassName((string)value);
                }

                return this.Driver.FindElementByClassName((string)value);
            }

            if (key == Core.By.ControlTypeProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElement(OpenQA.Selenium.By.XPath($@".//*[@ControlType='{value}']"));
                }

                return this.Driver.FindElement(OpenQA.Selenium.By.XPath($@".//*[@ControlType='{value}']"));
            }

            if (key == Core.By.LocalizedControlTypeProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElement(OpenQA.Selenium.By.XPath($@".//*[@LocalizedControlType='{value}']"));
                }

                return this.Driver.FindElement(OpenQA.Selenium.By.XPath($@".//*[@LocalizedControlType='{value}']"));
            }

            if (key == Core.By.NameProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementByName((string)value);
                }

                return this.Driver.FindElementByName((string)value);
            }

            if (key == Core.By.HelpTextProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElement(OpenQA.Selenium.By.XPath($@".//*[@HelpText='{value}']"));
                }

                return this.Driver.FindElement(OpenQA.Selenium.By.XPath($@".//*[@HelpText='{value}']"));
            }

            if (key == Core.By.AutomationIdProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementByAccessibilityId((string)value);
                }

                return this.Driver.FindElementByAccessibilityId((string)value);
            }

            if (key == Core.By.ParentRuntimeId)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementByXPath($".//*[parent::*[@RuntimeId='{value}']]");
                }

                return this.Driver.FindElementByXPath($".//*[parent::*[@RuntimeId='{value}']]");
            }

            if (key == Core.By.IsSelectedProperty)
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElementByXPath($@".//*[@IsSelected='{value}']");
                }

                return this.Driver.FindElementByXPath($@".//*[@IsSelected='{value}']");
            }

            if (key == "Parent")
            {
                if (this.windowsElement != null)
                {
                    return this.windowsElement.FindElement(OpenQA.Selenium.By.XPath($".//*[parent::*]"));
                }
            }

            throw new AtfSearchCriteriaException(key);
        }

        public bool Click(Core.MouseButton button)
        {
            return this.Click(button, 0, 0);
        }

        public bool Click(Core.MouseButton button, int xOffset, int yOffset)
        {
            this.TryBringIntoView();

            if (button == Core.MouseButton.Undefined)
            {
                throw new ArgumentException();
            }

            var a = new Actions(this.Driver);

            a.MoveToElement(this.windowsElement);

            if (xOffset != 0 || yOffset != 0)
            {
                a.MoveByOffset(xOffset, yOffset);
            }

            if (button == Core.MouseButton.Left)
            {
                a = a.Click();
            }
            else if (button == Core.MouseButton.Right)
            {
                a = a.ContextClick();
            }

            a.Build().Perform();

            return true;
        }

        public bool DoubleClick()
        {
            this.TryBringIntoView();

            new Actions(this.Driver).MoveToElement(this.windowsElement).DoubleClick().Build().Perform();
            return true;
        }

        public string GetPropertyValue(WindowsElement element, string propertyName)
        {
            this.TryBringIntoView();

            if (element != null)
            {
                var s = Stopwatch.StartNew();

                var propertyValue = element.GetAttribute(propertyName);

                s.Stop();

                return propertyValue;
            }

            return null;
        }

        public void MouseHover()
        {
            this.ElementContext.BringIntoViewFunc(this.ElementContext.WindowName);

            var a = new Actions(this.Driver);

            a.MoveToElement(this.windowsElement);

            a.Build().Perform();
        }

        public string GetPropertyValue(string propertyName)
        {
            return this.GetPropertyValue(this.windowsElement, propertyName);
        }

        public bool SendKeys(string keys)
        {
            this.TryBringIntoView();

            this.Logger.Trace("Send keys {keys} on element {runtimeId}", keys, this.RuntimeId);
            this.windowsElement.SendKeys(keys);
            return true;
        }

        public void TakeScreenshot(string path)
        {
            this.TryBringIntoView();

            this.Logger.Trace("Take screenshot of element Id: {runtimeId}, ClassName: {ClassName}", this.RuntimeId, this.ClassName);
            Screenshot screenshot = this.windowsElement.GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        public byte[] TakeScreenshot()
        {
            this.TryBringIntoView();

            this.Logger.Trace("Take screenshot of element Id: {runtimeId}, ClassName: {ClassName}", this.RuntimeId, this.ClassName);
            Screenshot screenshot = this.windowsElement.GetScreenshot();

            return screenshot.AsByteArray;
        }

        public void SelectAllShortcut()
        {
            this.Logger.Trace("Select All (Ctrl+A) pressed on element {runtimeId}", this.RuntimeId);
            this.PressModifiedKey(Core.Keys.Control, 'a');
        }

        public void CopyToClipboardShortcut()
        {
            this.Logger.Trace("Copy selection to clipboard (Ctrl+C) pressed on element {runtimeId}", this.RuntimeId);
            this.PressModifiedKey(Core.Keys.Control, 'c');
        }

        public void PasteFromClipboardShortcut()
        {
            this.Logger.Trace("Paste from clipboard (Ctrl+V) pressed on element '{runtimeId}'", this.RuntimeId);
            this.PressModifiedKey(Core.Keys.Control, 'v');
        }

        public void MoveCaretToBegin()
        {
            this.SelectAllShortcut();
            this.MoveCaretToLeft();
        }

        public void MoveCaretToEnd()
        {
            this.SelectAllShortcut();
            this.MoveCaretToRight();
        }

        public void MoveCaretToLeft()
        {
            this.windowsElement.SendKeys(OpenQA.Selenium.Keys.Left);
            this.AwaitingService.WaitForDefaultActionDelay();
        }

        public void MoveCaretToRight()
        {
            this.TryBringIntoView();

            this.windowsElement.SendKeys(OpenQA.Selenium.Keys.Right);
            this.AwaitingService.WaitForDefaultActionDelay();
        }

        public void PressEscape()
        {
            this.TryBringIntoView();

            this.windowsElement.SendKeys(OpenQA.Selenium.Keys.Escape);
            this.AwaitingService.WaitForDefaultActionDelay();
        }

        public void PressKey(Core.Keys key)
        {
            this.TryBringIntoView();

            this.windowsElement.SendKeys(KeyMap.ToSeleniumKey(key));
            this.AwaitingService.WaitForDefaultActionDelay();
        }

        public void PressModifiedKey(Core.Keys modifier, char c)
        {
            this.TryBringIntoView();

            this.Logger.Trace("{wrapper}: Press modified key pressed on element '{runtimeId}' '{modifier} + {c}'", nameof(AppiumUiItemWrapper), this.RuntimeId, modifier, c);
            this.windowsElement.SendKeys(KeyMap.ToSeleniumKey(modifier) + c.ToString() + KeyMap.ToSeleniumKey(modifier));
            this.AwaitingService.WaitForDefaultActionDelay();
        }

        public void PressModifiedCombo(Core.Keys[] keys)
        {
            this.TryBringIntoView();

            var s = string.Empty;
            foreach (var item in keys)
            {
                s += Enum.GetName(typeof(Core.Keys), item) + "+";
            }

            s = s.Substring(0, s.Length - 1);

            this.Logger.Trace("Press combo keys on element {runtimeId} '{s}'", this.RuntimeId, s);

            var combo = string.Empty;

            foreach (var key in keys.Reverse())
            {
                if (string.IsNullOrEmpty(combo))
                {
                    combo = KeyMap.ToSeleniumKey(key);
                }
                else
                {
                    combo = KeyMap.ToSeleniumKey(key) + combo + KeyMap.ToSeleniumKey(key);
                }
            }

            this.windowsElement.SendKeys(combo);

            Thread.Sleep(50);
        }

        public IScreenCoordinates Coordinates
        {
            get
            {
                this.TryBringIntoView();

                if (this.windowsElement != null)
                {
                    var r = this.windowsElement.Rect;

                    var x = r.X;
                    var y = r.Y;

                    return new AppiumScreenCoordinates(x, y);
                }

                return new AppiumScreenCoordinates(int.MaxValue, int.MaxValue);
            }
        }

        public IElementDimensions Dimensions
        {
            get
            {
                this.TryBringIntoView();

                if (this.windowsElement != null)
                {
                    var r = this.windowsElement.Rect;

                    var width = r.Width;
                    var height = r.Height;

                    return new AppiumElementDimensions(width, height);
                }

                return new AppiumElementDimensions(double.NaN, double.NaN);
            }
        }

        public string Name => this.GetPropertyValue(NamePropertyName);

        private string runtimeId;

        public string RuntimeId
        {
            get
            {
                if (string.IsNullOrEmpty(this.runtimeId))
                {
                    this.runtimeId = this.GetPropertyValue(RuntimeIdPropertyName);
                }

                return this.runtimeId;
            }
        }

        public bool IsEnabled
        {
            get
            {
                var value = this.GetPropertyValue(IsEnabledPropertyName);
                if (value == "True")
                {
                    return true;
                }

                return false;
            }
        }

        public string HelpText => this.GetPropertyValue(HelpTextPropertyName);

        public bool IsVisible
        {
            get
            {
                var value = this.GetPropertyValue("BoundingRectangle");
                if (string.IsNullOrEmpty(value) || value == this.notVisibleBoundingRectangle)
                {
                    return false;
                }

                return true;
            }
        }

        public string ClassName => this.GetPropertyValue(ClassNamePropertyName);
    }
}
