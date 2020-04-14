using Rtr.Atf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rtr.Atf.Appium
{
    internal static class KeyMap
    {
        private static readonly ReadOnlyDictionary<Keys, string> AppiumKeyMap =
            new ReadOnlyDictionary<Keys, string>(
                new Dictionary<Keys, string>()
                {
                    { Keys.ArrowDown, OpenQA.Selenium.Keys.ArrowDown },
                    { Keys.ArrowUp, OpenQA.Selenium.Keys.ArrowUp },
                    { Keys.Control, OpenQA.Selenium.Keys.Control },
                    { Keys.Shift, OpenQA.Selenium.Keys.Shift },
                    { Keys.End, OpenQA.Selenium.Keys.End },
                    { Keys.Enter, OpenQA.Selenium.Keys.Enter },
                    { Keys.Escape, OpenQA.Selenium.Keys.Escape },
                    { Keys.Home, OpenQA.Selenium.Keys.Home },
                    { Keys.Windows, OpenQA.Selenium.Keys.Meta },
                    { Keys.Alt, OpenQA.Selenium.Keys.LeftAlt },
                    { Keys.F4, OpenQA.Selenium.Keys.F4 },
                    { Keys.Tab, OpenQA.Selenium.Keys.Tab },
                });

        internal static string ToSeleniumKey(Keys key)
        {
            return AppiumKeyMap[key];
        }
    }
}
