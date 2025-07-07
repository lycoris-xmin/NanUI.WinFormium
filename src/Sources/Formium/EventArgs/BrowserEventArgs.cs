// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.EventArgs;

public class BrowserEventArgs : System.EventArgs
{
    public BrowserEventArgs(CefBrowser browser)
    {
        Browser = browser;
    }

    public CefBrowser Browser { get; }
}
