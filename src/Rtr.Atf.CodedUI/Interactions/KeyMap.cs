using Rtr.Atf.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WindowsInput.Native;

namespace Rtr.Atf.CodedUI
{
    internal class KeyMap
    {
        private static readonly ReadOnlyDictionary<Keys, VirtualKeyCode> CodedUiKeyMap =
            new ReadOnlyDictionary<Keys, VirtualKeyCode>(
                new Dictionary<Keys, VirtualKeyCode>()
                {
                    { Keys.ArrowDown, VirtualKeyCode.DOWN },
                    { Keys.ArrowUp, VirtualKeyCode.UP },
                    { Keys.Control, VirtualKeyCode.CONTROL },
                    { Keys.End, VirtualKeyCode.END },
                    { Keys.Enter, VirtualKeyCode.RETURN },
                    { Keys.Escape, VirtualKeyCode.ESCAPE },
                    { Keys.Home, VirtualKeyCode.HOME },
                    { Keys.Windows, VirtualKeyCode.LWIN },
                    { Keys.Alt, VirtualKeyCode.MENU },
                    { Keys.F4, VirtualKeyCode.F4 },
                    { Keys.Tab, VirtualKeyCode.TAB },
                });

        internal static VirtualKeyCode ToSeleniumKey(Keys key)
        {
            return CodedUiKeyMap[key];
        }
    }
}
