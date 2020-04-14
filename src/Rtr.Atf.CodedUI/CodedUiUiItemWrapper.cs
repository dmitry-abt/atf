using Microsoft.VisualStudio.TestTools.UITesting;
using NLog;
using Rtr.Atf.CodedUI.UI;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Automation;
using WindowsInput;
using WindowsInput.Native;

namespace Rtr.Atf.CodedUI
{
    public class CodedUiUiItemWrapper : IUiItemWrapper
    {
        public static readonly string NamePropertyName = "Name";

        public static readonly string RuntimeIdPropertyName = "RuntimeId";

        public static readonly string IsEnabledPropertyName = "IsEnabled";

        public static readonly string HelpTextPropertyName = "HelpText";

        public static readonly string ClassNamePropertyName = "ClassName";

        private readonly AutomationElement automationElement;

        private readonly IMouseSimulator mouse;
        private readonly IKeyboardSimulator keyboard;
        private readonly ILogger logger;
        private readonly ISettings settings;
        private readonly IAwaitingService awaitingService;

        private readonly int visualTreeNavigationRetryCount;

        public CodedUiUiItemWrapper(
            AutomationElement automationElement,
            IMouseSimulator mouseSimulator,
            IKeyboardSimulator keyboardSimulator,
            ILogger logger,
            ISettings settings,
            IAwaitingService awaitingService)
        {
            this.automationElement = automationElement ?? throw new ArgumentNullException(nameof(automationElement));
            this.mouse = mouseSimulator ?? throw new ArgumentNullException(nameof(mouseSimulator));
            this.keyboard = keyboardSimulator ?? throw new ArgumentNullException(nameof(keyboardSimulator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.awaitingService = awaitingService ?? throw new ArgumentNullException(nameof(awaitingService));

            this.visualTreeNavigationRetryCount = this.settings.VisualTreeNavigationRetryCount;
        }

        internal IUiItemWrapper FindFirst(TreeScope treeScope, Condition condition, IEnumerable<(string key, object value)> conditions)
        {
            this.logger.Trace("{wrapper}: Search first element with criteria: {criteria}", nameof(CodedUiUiItemWrapper), conditions);

            var s = Stopwatch.StartNew();

            AutomationElement foundElement = null;
            var attemptsCount = 0;

            while (foundElement == null && attemptsCount < this.visualTreeNavigationRetryCount)
            {
                attemptsCount++;
                foundElement = this.FindFirstInternal(new List<AutomationElement> { this.automationElement }, treeScope, condition, conditions);

                if (foundElement == null)
                {
                    this.logger.Trace("Couldn't find element on {attempt} attempt", attemptsCount);
                    this.awaitingService.WaitForDefaultRetryDelay();
                }
            }

            s.Stop();

            if (foundElement == null)
            {
                this.logger.Error("{wrapper}: Failed search first element with criteria: {criteria}. Elepsed time: {time}ms", nameof(CodedUiUiItemWrapper), conditions, s.ElapsedMilliseconds);
                throw new AtfNavigationException($"Failed search first element with criteria: {conditions}");
            }

            this.logger.Trace("{wrapper}: Completed search first element with criteria: {criteria}. Elapsed time: {time}ms", nameof(CodedUiUiItemWrapper), conditions, s.ElapsedMilliseconds);

            return new CodedUiUiItemWrapper(foundElement, this.mouse, this.keyboard, this.logger, this.settings, this.awaitingService);
        }

        private AutomationElement FindFirstInternal(IEnumerable<AutomationElement> elements, TreeScope treeScope, Condition condition, IEnumerable<(string key, object value)> conditions)
        {
            try
            {
                if (elements == null || !elements.Any() || elements.All(e => e == null))
                {
                    return null;
                }

                if (elements.Count() == 1 && treeScope == TreeScope.Children)
                {
                    return elements.First().FindFirst(TreeScope.Children, condition);
                }

                var conditionalWalker = new TreeWalker(condition);

                foreach (var item in elements)
                {
                    var items = item.FindAll(TreeScope.Children, Condition.TrueCondition);

                    foreach (var i in items)
                    {
                        var automationElement = i as AutomationElement;
                        var isOk = this.CheckConditions(automationElement, conditions);
                        if (isOk)
                        {
                            return automationElement;
                        }
                    }
                }

                if (treeScope != TreeScope.Children)
                {
                    var children = new List<AutomationElement>();

                    foreach (var item in elements)
                    {
                        var itemChildren = item.FindAll(TreeScope.Children, Condition.TrueCondition);
                        foreach (var i in itemChildren)
                        {
                            children.Add((AutomationElement)i);
                        }
                    }

                    return this.FindFirstInternal(children, treeScope, condition, conditions);
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }

            return null;
        }

        internal IEnumerable<IUiItemWrapper> FindAll(TreeScope treeScope, Condition condition, IEnumerable<(string key, object value)> conditions)
        {
            this.logger.Trace("{wrapper}: Search all elements with criteria: {criteria}", nameof(CodedUiUiItemWrapper), conditions);

            var s = Stopwatch.StartNew();

            IEnumerable<AutomationElement> foundItems = null;

            var attemptsCount = 0;

            while ((foundItems == null || !foundItems.Any()) && attemptsCount < this.visualTreeNavigationRetryCount)
            {
                attemptsCount++;

                try
                {
                    foundItems = this.FindAllInternal(new List<AutomationElement> { this.automationElement }, treeScope, condition, conditions);

                    if (foundItems == null || !foundItems.Any())
                    {
                        this.logger.Trace("Couldn't find elements on {attempt} attempt", attemptsCount);
                        this.awaitingService.WaitForDefaultRetryDelay();
                    }
                }
                catch (Exception e)
                {
                    this.logger.Error(e);
                }
            }

            s.Stop();

            this.logger.Trace("{wrapper}: Completed search all elements with criteria: {criteria}. Elapsed time: {time} ms", nameof(CodedUiUiItemWrapper), conditions, s.ElapsedMilliseconds);

            return foundItems.Select(item => new CodedUiUiItemWrapper(item, this.mouse, this.keyboard, this.logger, this.settings, this.awaitingService));
        }

        private IEnumerable<AutomationElement> FindAllInternal(IEnumerable<AutomationElement> elements, TreeScope treeScope, Condition condition, IEnumerable<(string key, object value)> conditions)
        {
            if (elements == null || !elements.Any() || elements.All(e => e == null))
            {
                yield break;
            }

            if (elements.Count() == 1 && treeScope == TreeScope.Children)
            {
                foreach (var item in elements.First().FindAll(TreeScope.Children, condition))
                {
                    yield return (AutomationElement)item;
                }
            }

            var conditionalWalker = new TreeWalker(condition);

            var children = new List<AutomationElement>();

            foreach (var item in elements)
            {
                var allChildren = item.FindAll(TreeScope.Children, Condition.TrueCondition);

                foreach (var i in allChildren)
                {
                    var ae = (AutomationElement)i;
                    var isOk = this.CheckConditions(ae, conditions);
                    if (isOk)
                    {
                        yield return ae;
                    }
                }

                if (treeScope != TreeScope.Children)
                {
                    foreach (var i in allChildren)
                    {
                        children.Add((AutomationElement)i);
                    }
                }
            }

            foreach (var item in this.FindAllInternal(children, treeScope, condition, conditions))
            {
                yield return item;
            }

            yield break;
        }

        public bool Click(Core.MouseButton button, int xOffset, int yOffset)
        {
            (double x, double y) = (
                this.Coordinates.X + (this.Dimensions.Width / 2) + xOffset,
                this.Coordinates.Y + (this.Dimensions.Height / 2) + yOffset);

            Mouse.Hover(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));

            if (button == Core.MouseButton.Left)
            {
                this.mouse.LeftButtonClick();
            }
            else if (button == Core.MouseButton.Right)
            {
                this.mouse.RightButtonClick();
            }

            return true;
        }

        public bool Click(Core.MouseButton button)
        {
            return this.Click(button, 0, 0);
        }

        public bool DoubleClick()
        {
            throw new NotImplementedException();
        }

        public string GetPropertyValue(string propertyName)
        {
            return this.GetPropertyValue(this.automationElement, propertyName);
        }

        public string GetPropertyValue(AutomationElement automationElement, string propertyName)
        {
            var s = Stopwatch.StartNew();

            var mapped = PropertyMapper.ToCodedUiAutomationProperty(propertyName);

            string result = null;

            var value = automationElement.GetCurrentPropertyValue(mapped);

            if (value is string)
            {
                result = value as string;
            }
            else if (value is int)
            {
                result = Convert.ToString(value);
            }
            else if (value is bool)
            {
                result = (bool)value ? "True" : "False";
            }
            else
            {
                if (mapped == AutomationElement.RuntimeIdProperty)
                {
                    var bytes = value as int[];
                    result = PropertyHelper.RuntimeIdBytesToString(bytes);
                }

                if (mapped == ExpandCollapsePattern.ExpandCollapseStateProperty)
                {
                    var state = (ExpandCollapseState)value;
                    result = PropertyHelper.ExpandCollapseStateToString(state);
                }

                if (mapped == TogglePattern.ToggleStateProperty)
                {
                    result = ((ToggleState)value) == ToggleState.On ? "1" : "0";
                }
            }

            s.Stop();

            // logger.Trace("Get property {prop}, value is {result}. Elapsed time: {time}ms", propertyName, result, s.ElapsedMilliseconds);
            return result;
        }

        public IScreenCoordinates Coordinates => new CodedUiScreenCoordinates(this.automationElement.Current.BoundingRectangle.X, this.automationElement.Current.BoundingRectangle.Y);

        public IElementDimensions Dimensions => new CodedUiElementDimensions(this.automationElement.Current.BoundingRectangle.Width, this.automationElement.Current.BoundingRectangle.Height);

        public string Name => this.GetPropertyValue(NamePropertyName);

        public string RuntimeId => this.GetPropertyValue(RuntimeIdPropertyName);

        public string HelpText => this.GetPropertyValue(HelpTextPropertyName);

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

        public string ClassName => this.GetPropertyValue(ClassNamePropertyName);

        private Rectangle notVisibleBoundingRectange = new Rectangle(0, 0, 0, 0);

        public bool IsVisible
        {
            get
            {
                var b = this.automationElement.TryGetClickablePoint(out Point t);
                if (t.IsEmpty)
                {
                    return false;
                }

                var value = (Rectangle)this.automationElement.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);
                this.logger.Trace("Element bounding rectangle is {value}", value);
                var isOffscreen = this.automationElement.Current.IsOffscreen;
                value = (Rectangle)this.automationElement.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                this.logger.Trace("Element TryGetClickablePoint result is {b}", b);
                this.logger.Trace("Element bounding rectangle is {value}", value);
                this.logger.Trace("Element is offscreen - {isOffscreen}", isOffscreen);

                this.logger.Trace("Element clickable point ({X}, {Y})", t.X, t.Y);

                return value != this.notVisibleBoundingRectange && !isOffscreen;
            }
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
            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.KeyPress(VirtualKeyCode.LEFT);

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public void MoveCaretToRight()
        {
            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.KeyPress(VirtualKeyCode.RIGHT);

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public void PasteFromClipboardShortcut()
        {
            this.logger.Trace("Paste from clipboard (Ctrl+V) pressed on element {runtimeId}", this.RuntimeId);

            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public void CopyToClipboardShortcut()
        {
            this.logger.Trace("Copy selection to clipboard (Ctrl+C) pressed on element {runtimeId}", this.RuntimeId);

            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public void SelectAllShortcut()
        {
            this.logger.Trace("Select All (Ctrl+A) pressed on element {runtimeId}", this.RuntimeId);

            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public bool SendKeys(string keys)
        {
            this.logger.Trace("Send keys '{keys}' on element {runtimeId}", keys, this.RuntimeId);

            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.TextEntry(keys);

            this.awaitingService.WaitForDefaultActionDelay();

            return true;
        }

        public void PressEscape()
        {
            this.logger.Trace("Press Escape on element {runtimeId}", this.RuntimeId);

            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.KeyPress(VirtualKeyCode.ESCAPE);

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public void TakeScreenshot(string path)
        {
            byte[] imageBytes;

            var rect = new Rectangle(this.Coordinates.X, this.Coordinates.Y, Convert.ToInt32(this.Dimensions.Width), Convert.ToInt32(this.Dimensions.Height));

            using (var bitmap = new Bitmap(rect.Width == 0 ? 1 : rect.Width, rect.Height == 0 ? 1 : rect.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(rect.Left, rect.Top), Point.Empty, rect.Size);
                }

                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    imageBytes = stream.ToArray();
                }
            }

            this.ByteArrayToFile(path, imageBytes);
        }

        private bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        public AutomationElement GetParent()
        {
            var parent = TreeWalker.ControlViewWalker.GetParent(this.automationElement);
            return parent;
        }

        private bool CheckConditions(AutomationElement ae, IEnumerable<(string key, object value)> conditions)
        {
            var isOk = true;
            foreach (var (key, value) in conditions)
            {
                if (!isOk)
                {
                    break;
                }

                if (key == By.ClassNameProperty)
                {
                    if (ae.Current.ClassName != (string)value)
                    {
                        isOk = false;
                    }
                }

                if (key == By.AutomationIdProperty)
                {
                    if (ae.Current.AutomationId != (string)value)
                    {
                        isOk = false;
                    }
                }

                if (key == By.HelpTextProperty)
                {
                    if (ae.Current.HelpText != (string)value)
                    {
                        isOk = false;
                    }
                }

                if (key == By.NameProperty)
                {
                    if (ae.Current.Name != (string)value)
                    {
                        isOk = false;
                    }
                }

                if (key == By.RuntimeIdProperty)
                {
                    if (this.GetPropertyValue(ae, By.RuntimeIdProperty) != (string)value)
                    {
                        isOk = false;
                    }
                }
            }

            return isOk;
        }

        public void PressKey(Core.Keys key)
        {
            this.logger.Trace("Send key '{key}' on element {runtimeId}", key, this.RuntimeId);

            if (!this.automationElement.Current.HasKeyboardFocus)
            {
                this.automationElement.SetFocus();
            }

            this.keyboard.KeyPress(KeyMap.ToSeleniumKey(key));

            this.awaitingService.WaitForDefaultActionDelay();
        }

        public void PressModifiedKey(Core.Keys modifier, char c)
        {
            var mod = KeyMap.ToSeleniumKey(modifier);

            if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(c.ToString().ToUpper()))
            {
                Enum.TryParse<VirtualKeyCode>("VK_" + c.ToString().ToUpper(), out var res);
                this.keyboard.ModifiedKeyStroke(mod, res);
            }
            else
            {
                throw new ArgumentException($"Unsupported char {c}");
            }
        }

        public void PressModifiedCombo(Core.Keys[] keys)
        {
            var mapped = keys.Select(k => KeyMap.ToSeleniumKey(k));

            var last = mapped.Last();
            foreach (var item in mapped)
            {
                if (item == last)
                {
                    continue;
                }

                this.keyboard.KeyDown(item);
            }

            this.keyboard.KeyPress(last);

            foreach (var item in mapped.Reverse())
            {
                if (item == last)
                {
                    continue;
                }

                this.keyboard.KeyUp(item);
            }
        }

        public void MouseHover()
        {
            throw new NotImplementedException();
        }

        public byte[] TakeScreenshot()
        {
            throw new NotImplementedException();
        }
    }
}
