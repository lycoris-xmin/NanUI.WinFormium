// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.Browser.@base;

namespace WinFormium.Sources.Formium.BrowserHandlerImplements;
public abstract class KeyboardHandler : IKeyboardHandler
{
    bool IKeyboardHandler.OnPreKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent, out bool isKeyboardShortcut)
    {
        return OnPreKeyEvent(browser, keyEvent, osEvent, out isKeyboardShortcut);
    }

    bool IKeyboardHandler.OnKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent)
    {
        return OnKeyEvent(browser, keyEvent, osEvent);
    }

    internal protected virtual bool OnPreKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent, out bool isKeyboardShortcut)
    {
        isKeyboardShortcut = false;
        return false;
    }

    internal protected virtual bool OnKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent)
    {
        return false;
    }
}
