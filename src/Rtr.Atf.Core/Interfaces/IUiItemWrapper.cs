namespace Rtr.Atf.Core
{
    /// <summary>
    /// Wrapper interface on UI item.
    /// </summary>
    public interface IUiItemWrapper
    {
        /// <summary>
        /// Gets info about position of item on screen.
        /// </summary>
        IScreenCoordinates Coordinates { get; }

        /// <summary>
        /// Gets info about width and height of an item on screen.
        /// </summary>
        IElementDimensions Dimensions { get; }

        /// <summary>
        /// Gets value of <see cref="Name"/> property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets value of <see cref="RuntimeId"/> property.
        /// </summary>
        string RuntimeId { get; }

        /// <summary>
        /// Gets value of <see cref="HelpText"/> property.
        /// </summary>
        string HelpText { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="IsEnabled"/>
        /// property is <see cref="bool.True"/> or <see cref="bool.False"/>.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether item is visible or not.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets value of <see cref="ClassName"/> property.
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets value of a given property.
        /// </summary>
        /// <param name="propertyName">A name of property.</param>
        /// <returns>Property's value.</returns>
        string GetPropertyValue(string propertyName);

        /// <summary>
        /// Takes screenshot of an item and saves it to given folder.
        /// </summary>
        /// <param name="path">A folder to save screenshot.</param>
        void TakeScreenshot(string path);

        /// <summary>
        /// Takes screenshot of an item.
        /// </summary>
        /// <returns><see cref="byte"/> array which represents item screenshot.</returns>
        byte[] TakeScreenshot();

        /// <summary>
        /// Emulates pressing Ctrl-A keyboard combination.
        /// </summary>
        void SelectAllShortcut();

        /// <summary>
        /// Emulates pressing Ctrl-C keyboard combination.
        /// </summary>
        void CopyToClipboardShortcut();

        /// <summary>
        /// Emulates pressing Ctrl-V keyboard combination.
        /// </summary>
        void PasteFromClipboardShortcut();

        /// <summary>
        /// Emulates pressing keyboard keys for setting caret in the beginning of text field.
        /// Designed to be used on text input items.
        /// </summary>
        void MoveCaretToBegin();

        /// <summary>
        /// Emulates pressing keyboard keys for setting caret in the end of text field.
        /// Designed to be used on text input items.
        /// </summary>
        void MoveCaretToEnd();

        /// <summary>
        /// Emulates pressing keyboard keys for moving caret in the left direction.
        /// Designed to be used on text input items.
        /// </summary>
        void MoveCaretToLeft();

        /// <summary>
        /// Emulates pressing keyboard keys for moving caret in the right direction.
        /// Designed to be used on text input items.
        /// </summary>
        void MoveCaretToRight();

        /// <summary>
        /// Emulates pressing specific key.
        /// </summary>
        /// <param name="key">A key to press.</param>
        void PressKey(Keys key);

        /// <summary>
        /// Hovers mouse cursor over item.
        /// </summary>
        void MouseHover();

        /// <summary>
        /// Emulates pressing keyboard combation of single modifier and single literal keys
        /// (Ctrl-A, Ctrl-C, Alt-B, etc.)
        /// </summary>
        /// <param name="modifier">A modifier key to be pressed.</param>
        /// <param name="c">A common key to be pressed.</param>
        void PressModifiedKey(Keys modifier, char c);

        /// <summary>
        /// Emulates pressing keyboard combination of multiple modifier keys (ex: Ctrl-Alt-Del).
        /// </summary>
        /// <param name="keys">Array of keys to be pressed.</param>
        void PressModifiedCombo(Keys[] keys);

        /// <summary>
        /// Single click in the middle of wrapped item.
        /// </summary>
        /// <param name="button">A mouse button to be clicked.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool Click(MouseButton button);

        /// <summary>
        /// Single click in the middle of wrapped item with offset.
        /// </summary>
        /// <param name="button">A mouse button to be clicked.</param>
        /// <param name="xOffset">Offset by X coordinate.</param>
        /// <param name="yOffset">Offset by Y coordinate.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool Click(MouseButton button, int xOffset, int yOffset);

        /// <summary>
        /// Double click in the middle of wrapped item.
        /// </summary>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool DoubleClick();

        /// <summary>
        /// Send to item a sequence of keyboard keys. Imitates keyboard input.
        /// </summary>
        /// <param name="keys">A sequence of keys to be sent.</param>
        /// <returns>A value indicating whether operation was successful or not.</returns>
        bool SendKeys(string keys);
    }
}
