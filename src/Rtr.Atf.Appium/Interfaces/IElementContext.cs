using NLog;
using OpenQA.Selenium.Appium.Windows;
using Rtr.Atf.Core;
using System;

namespace Rtr.Atf.Appium
{
    internal interface IElementContext
    {
        string WindowName { get; }

        ILogger Logger { get; }

        IAwaitingService AwaitingService { get; }

        ISettings Settings { get; }

        AppiumUiItemWrapperFactory WrapperFactory { get; }

        WindowsDriver<WindowsElement> Driver { get; }

        Action<string> BringIntoViewFunc { get; }
    }
}
