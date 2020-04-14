using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides high level, agnostic to UI automation framework representation of UI item.
    /// </summary>
    public class Element
    {
        /// <summary>
        /// A service for locating UI items and navigating
        /// among several running applications.
        /// </summary>
        private readonly IUiNavigationProvider uiNavigationProvider;

        /// <summary>
        /// A service for helping locating UI items in a visual tree
        /// and initializing instances of <see cref="Element"/> for located items.
        /// </summary>
        private readonly IElementFactory elementFactory;

        /// <summary>
        /// A service for awaiting various conditions to match.
        /// </summary>
        private readonly IAwaitingService awaitingService;

        /// <summary>
        /// A logging service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="itemWrapper">Wrapped UI item by specific UI automation framework.</param>
        /// <param name="uiNavigationProvider">
        /// A service for locating UI items and navigating
        /// among several running applications.
        /// </param>
        /// <param name="elementFactory">
        /// A service for helping locating UI items in a visual tree
        /// and initializing instances of <see cref="Element"/> for located items.
        /// </param>
        /// <param name="awaitingService">A service for awaiting various conditions to match.</param>
        /// <param name="logger">A loggind service.</param>
        protected internal Element(
            IUiItemWrapper itemWrapper,
            IUiNavigationProvider uiNavigationProvider,
            IElementFactory elementFactory,
            IAwaitingService awaitingService,
            ILogger logger)
        {
            this.Instance = itemWrapper ?? throw new ArgumentNullException(nameof(itemWrapper));

            this.uiNavigationProvider = uiNavigationProvider ?? throw new ArgumentNullException(nameof(uiNavigationProvider));
            this.elementFactory = elementFactory ?? throw new ArgumentNullException(nameof(elementFactory));
            this.awaitingService = awaitingService ?? throw new ArgumentNullException(nameof(awaitingService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.DefaultSearchConditions = new ReadOnlyCollection<(string key, object value)>(elementFactory.GetDefaultSearchConditions(this.GetType()).ToList());
        }

        /// <summary>
        /// Gets wrapper of UI item element represents.
        /// </summary>
        public IUiItemWrapper Instance { get; }

        /// <summary>
        /// Gets name of element.
        /// </summary>
        public string Name => this.Instance.Name;

        /// <summary>
        /// Gets runtime Id of element.
        /// </summary>
        public string RuntimeId => this.Instance.RuntimeId;

        /// <summary>
        /// Gets class name of element.
        /// </summary>
        public string ClassName => this.Instance.ClassName;

        /// <summary>
        /// Gets info about position of element on screen.
        /// </summary>
        public IScreenCoordinates Coordinates => this.Instance.Coordinates;

        /// <summary>
        /// Gets info about width and height of an element on screen.
        /// </summary>
        public IElementDimensions Dimensions => this.Instance.Dimensions;

        /// <summary>
        /// Gets a value indicating whether element is visible or not.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                this.awaitingService.WaitForDefaultActionDelay();
                return this.Instance.IsVisible;
            }
        }

        /// <summary>
        /// Gets a value indicating whether element is enabled or not.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                this.awaitingService.WaitForDefaultActionDelay();
                return this.Instance.IsEnabled;
            }
        }

        /// <summary>
        /// Gets default search conditions which were used to locate this element.
        /// Used for debug purposes.
        /// </summary>
        private ReadOnlyCollection<(string, object)> DefaultSearchConditions { get; }

        /// <summary>
        /// Finds all elements in subtree that match given single condition.
        /// </summary>
        /// <param name="conditionKey">A condition key for search.</param>
        /// <param name="conditionValue">A condition value for search.</param>
        /// <returns>A read-only collection of all found elements.</returns>
        public ReadOnlyCollection<Element> FindAllElements(string conditionKey, object conditionValue)
        {
            return this.FindAllElements((conditionKey, conditionValue));
        }

        /// <summary>
        /// Finds all elements of type <typeparamref name="T"/> in subtree that match given single condition.
        /// </summary>
        /// <typeparam name="T">Type of elements to find.</typeparam>
        /// <param name="conditionKey">A condition key for search.</param>
        /// <param name="conditionValue">A condition value for search.</param>
        /// <returns>A read-only collection of all found elements.</returns>
        public ReadOnlyCollection<T> FindAllElements<T>(string conditionKey, object conditionValue)
            where T : Element
        {
            return this.FindAllElements<T>((conditionKey, conditionValue));
        }

        /// <summary>
        /// Finds all elements in subtree that match given multiple conditions.
        /// </summary>
        /// <param name="conditions">A collection of conditions found elements have to match.</param>
        /// <returns>A read-only collection of all found elements.</returns>
        public ReadOnlyCollection<Element> FindAllElements(params (string, object)[] conditions)
        {
            return this.FindAllElements<Element>(conditions);
        }

        /// <summary>
        /// Finds all elements of type <typeparamref name="T"/> in subtree that match given multiple conditions.
        /// </summary>
        /// <typeparam name="T">Type of elements to find.</typeparam>
        /// <param name="conditions">A set of conditions found elements have to match.</param>
        /// <returns>A read-only collection of all found elements.</returns>
        public ReadOnlyCollection<T> FindAllElements<T>(params (string key, object value)[] conditions)
            where T : Element
        {
            this.awaitingService.WaitForDefaultActionDelay();

            var result = new List<T>();

            var newConditions = this.UpdateWithDefaultConditions<T>(conditions);
            IEnumerable<IUiItemWrapper> items = this.uiNavigationProvider.FindAll(this, newConditions);
            foreach (var ae in items)
            {
                var element = this.CreateElementInternal<T>(ae);
                result.Add(element);
            }

            return new ReadOnlyCollection<T>(result);
        }

        /// <summary>
        /// Finds first element of type <typeparamref name="T"/> in subtree that match given multiple conditions.
        /// </summary>
        /// <typeparam name="T">Type of element to find.</typeparam>
        /// <param name="conditions">A set of conditions found element has to match.</param>
        /// <returns>A found element.</returns>
        public T FindElement<T>(params (string, object)[] conditions)
            where T : Element
        {
            this.awaitingService.WaitForDefaultActionDelay();

            var newConditions = this.UpdateWithDefaultConditions<T>(conditions);
            IUiItemWrapper item = this.uiNavigationProvider.FindFirst(this, newConditions);

            return this.CreateElementInternal<T>(item);
        }

        /// <summary>
        /// Finds first element of type <typeparamref name="T"/> in subtree that match given single condition.
        /// </summary>
        /// <typeparam name="T">Type of element to find.</typeparam>
        /// <param name="conditionKey">A condition key for search.</param>
        /// <param name="conditionValue">A condition value for search.</param>
        /// <returns>A found element.</returns>
        public T FindElement<T>(string conditionKey, object conditionValue)
            where T : Element
        {
            return this.FindElement<T>((conditionKey, conditionValue));
        }

        /// <summary>
        /// Finds first element  in subtree that match given single condition.
        /// </summary>
        /// <param name="conditions">A set of conditions found element has to match.</param>
        /// <returns>A found element.</returns>
        public Element FindElement(params (string, object)[] conditions)
        {
            return this.FindElement<Element>(conditions);
        }

        /// <summary>
        /// Finds first element in subtree that match given single condition.
        /// </summary>
        /// <param name="conditionKey">A condition key for search.</param>
        /// <param name="conditionValue">A condition value for search.</param>
        /// <returns>A found element.</returns>
        public Element FindElement(string conditionKey, object conditionValue)
        {
            return this.FindElement<Element>(conditionKey, conditionValue);
        }

        /// <summary>
        /// Clicks on in the middle of element with offset with default mouse button.
        /// </summary>
        /// <param name="xOffset">Offset from the middle by X-axis.</param>
        /// <param name="yOffset">Offset from the middle by Y-axis.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool Click(int xOffset, int yOffset)
        {
            return this.Instance.Click(MouseButton.Left, xOffset, yOffset);
        }

        /// <summary>
        /// Clicks on in the middle of element with default mouse button.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool Click()
        {
            return this.Click(MouseButton.Left);
        }

        /// <summary>
        /// Clicks on in the middle of element with default mouse button.
        /// </summary>
        /// <param name="maximumAwaitTime">Maximum amount of time to await before element becomes enabled.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool Click(TimeSpan maximumAwaitTime)
        {
            return this.Click(MouseButton.Left, maximumAwaitTime);
        }

        /// <summary>
        /// Clicks on in the middle of element with given mouse button.
        /// </summary>
        /// <param name="mouseButton">A ouse button to click.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool Click(MouseButton mouseButton)
        {
            this.WaitForEnabled();

            return this.Instance.Click(mouseButton);
        }

        /// <summary>
        /// Clicks on in the middle of element with given mouse button.
        /// </summary>
        /// <param name="mouseButton">A ouse button to click.</param>
        /// <param name="maximumAwaitTime">Maximum amount of time to await before element becomes enabled.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool Click(MouseButton mouseButton, TimeSpan maximumAwaitTime)
        {
            this.WaitForEnabled(maximumAwaitTime);

            return this.Instance.Click(mouseButton);
        }

        /// <summary>
        /// Double clicks default mouse button.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        public bool DoubleClick()
        {
            this.logger.Trace("DoubleClick");
            this.awaitingService.WaitFor(() => this.IsEnabled);
            return this.Instance.DoubleClick();
        }

        /// <summary>
        /// Sends sequence of keyboard keys to an element.
        /// </summary>
        /// <param name="keys">A sequence of keys.</param>
        /// <returns>An element itself.</returns>
        public Element SendKeys(string keys)
        {
            this.awaitingService.WaitForDefaultActionDelay();
            var result = this.Instance.SendKeys(keys);
            return this;
        }

        /// <summary>
        /// Emulates pressing specific key.
        /// </summary>
        /// <param name="key">A key to press.</param>
        /// <returns>An element itself.</returns>
        public Element PressKey(Keys key)
        {
            this.Instance.PressKey(key);
            return this;
        }

        /// <summary>
        /// Takes screenshot of a element and saves it to given location.
        /// </summary>
        /// <param name="path">A location to save screenshot.</param>
        public void TakeScreenshot(string path)
        {
            this.Instance.TakeScreenshot(path);
        }

        /// <summary>
        /// Takes screenshot of an item.
        /// </summary>
        /// <returns><see cref="byte"/> array which represents item screenshot.</returns>
        public byte[] TakeScreenshot()
        {
            return this.Instance.TakeScreenshot();
        }

        /// <summary>
        /// Awaits until element becomes enabled.
        /// </summary>
        public void WaitForEnabled() => this.awaitingService.WaitFor(() => this.IsEnabled);

        /// <summary>
        /// Awaits until element becomes enabled.
        /// </summary>
        /// <param name="maximumTime">A maximum amount of time to await.</param>
        public void WaitForEnabled(TimeSpan maximumTime) => this.awaitingService.WaitFor(() => this.IsEnabled, maximumTime);

        /// <summary>
        /// Attempts to close window element belongs.
        /// </summary>
        public void TryCloseWindow()
        {
            var elementExists = true;
            while (elementExists)
            {
                try
                {
                    var className = this.Instance.GetPropertyValue("ClassName");
                    this.Instance.PressModifiedCombo(new Keys[] { Keys.Alt, Keys.F4 });
                }
                catch (Exception)
                {
                    elementExists = false;
                }
            }
        }

        /// <summary>
        /// Hovers mouse cursor over element and gets its tooltip element.
        /// </summary>
        /// <returns>Found tooltip element.</returns>
        public Element GetTooltip()
        {
            this.awaitingService.WaitForDefaultActionDelay();

            this.Instance.MouseHover();
            this.awaitingService.WaitFor(TimeSpan.FromSeconds(1));
            var el = this.CreateElementInternal<Element>(this.uiNavigationProvider.GetAppRoot(this.uiNavigationProvider.CurrentWindowTitle));
            var popup = el.FindElement(By.ClassNameProperty, "Popup");
            return popup;
        }

        /// <summary>
        /// Compares element's screenshot with provided image.
        /// </summary>
        /// <param name="imageStream">Image to compare with element.</param>
        /// <returns>Floating-point value from 0 (equal) to (diffent).</returns>
        public float CompareToImage(Stream imageStream)
        {
            this.logger.Trace("Compare images:");

            // https://rosettacode.org/wiki/Percentage_difference_between_images#C.23
            var s = Stopwatch.StartNew();

            var temporaryScreenshotPath = $"{Guid.NewGuid().ToString()}.png";

            float result;

            try
            {
                this.TakeScreenshot(temporaryScreenshotPath.ToString());

                using (var img1 = new Bitmap(imageStream))
                {
                    using (var img2 = new Bitmap(temporaryScreenshotPath))
                    {
                        // logger.Trace("    {path}", path);
                        this.logger.Trace("    {path}", temporaryScreenshotPath);

                        if (img1.Size != img2.Size)
                        {
                            s.Stop();
                            this.logger.Trace("    Images are different sizes: {i1} and {i2}", img1.Size, img2.Size);
                            this.logger.Trace("    Image comparison finished in {time} ms", s.ElapsedMilliseconds);

                            this.logger.Trace("    result value is: {result}", 1);

                            return 1;
                        }

                        float diff = 0;

                        for (int y = 0; y < img1.Height; y++)
                        {
                            for (int x = 0; x < img1.Width; x++)
                            {
                                Color pixel1 = img1.GetPixel(x, y);
                                Color pixel2 = img2.GetPixel(x, y);

                                diff += Math.Abs(pixel1.R - pixel2.R);
                                diff += Math.Abs(pixel1.G - pixel2.G);
                                diff += Math.Abs(pixel1.B - pixel2.B);
                            }
                        }

                        result = (diff / 255) / (img1.Width * img1.Height * 3);

                        this.logger.Trace("    diff variable value is: {diff}", diff);
                        this.logger.Trace("    result value is: {result}", result);

                        this.logger.Trace("    Image comparison finished in {time} ms", s.ElapsedMilliseconds);
                    }
                }
            }
            finally
            {
                s.Stop();
                File.Delete(temporaryScreenshotPath);
            }

            return result;
        }

        /// <summary>
        /// Tries to find first element of type <typeparamref name="T"/> in subtree that match given multiple conditions.
        /// Otherwise, returns null.
        /// </summary>
        /// <typeparam name="T">Type of element to find.</typeparam>
        /// <param name="conditions">A set of conditions found elements have to match.</param>
        /// <returns>A found element.</returns>
        protected T TryFindElement<T>(params (string, object)[] conditions)
            where T : Element
        {
            this.logger.Trace("Internal call FindElement");

            T result = null;

            try
            {
                result = this.FindElement<T>(conditions);
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }

            return result;
        }

        /// <summary>
        /// Finds parent element relative to current element.
        /// </summary>
        /// <returns>Found element.</returns>
        protected Element FindParent()
        {
            var element = this.FindElement("Parent", string.Empty);
            return element;
        }

        /// <summary>
        /// Gets popup element. Use to get drop-down list of a combo box.
        /// </summary>
        /// <returns>Found popup element.</returns>
        protected Element GetPopup()
        {
            this.awaitingService.WaitForDefaultActionDelay();

            var el = this.CreateElementInternal<Element>(this.uiNavigationProvider.GetAppRoot(this.uiNavigationProvider.CurrentWindowTitle));
            var popup = el.FindElement(By.ClassNameProperty, "Popup");
            return popup;
        }

        /// <summary>
        /// Handles creation of element of given type for UI item.
        /// </summary>
        /// <typeparam name="T">Type of element to create.</typeparam>
        /// <param name="itemWrapper">UI item wrapper.</param>
        /// <returns>Created element.</returns>
        private T CreateElementInternal<T>(IUiItemWrapper itemWrapper)
            where T : Element
        {
            var type = typeof(T);
            if (type == typeof(Element))
            {
                return new Element(
                    itemWrapper,
                    this.uiNavigationProvider,
                    this.elementFactory,
                    this.awaitingService,
                    this.logger) as T;
            }

            return this.elementFactory.CreateElement<T>(itemWrapper, this.uiNavigationProvider, this.elementFactory, this.awaitingService, this.logger);
        }

        /// <summary>
        /// Updates given search conditions with default conditions for type if needed.
        /// </summary>
        /// <typeparam name="T">Type of item to be searched.</typeparam>
        /// <param name="conditions">Search conditions to be updated.</param>
        /// <returns>Updated conditions.</returns>
        private IEnumerable<(string, object)> UpdateWithDefaultConditions<T>(IEnumerable<(string, object)> conditions)
            where T : Element
        {
            var newConditions = new List<(string key, object value)>();

            var defaultConditions = this.elementFactory.GetDefaultSearchConditions(typeof(T));

            foreach (var (key, value) in defaultConditions)
            {
                this.logger.Trace("Default search conditions for type {type}: ({key}, {value})", typeof(T), key, value);
            }

            if (conditions.Any())
            {
                var dict = new Dictionary<string, object>();

                foreach (var (key, value) in defaultConditions)
                {
                    dict[key] = value;
                }

                foreach (var (key, value) in conditions)
                {
                    if (dict.ContainsKey(key))
                    {
                        this.logger.Trace("Override search criteria ({0}, {1}) with ({2}, {3})", key, dict[key], key, value);
                    }

                    dict[key] = value;
                }

                foreach (var d in dict)
                {
                    newConditions.Add((d.Key, d.Value));
                }
            }
            else
            {
                newConditions.AddRange(defaultConditions);
            }

            return newConditions;
        }
    }
}
