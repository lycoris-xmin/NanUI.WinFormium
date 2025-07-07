// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.EventArgs;

public class PageLoadEndEventArgs : System.EventArgs
{
    public CefBrowser Browser { get; }
    public CefFrame Frame { get; }
    public int HttpStatusCode { get; }

    public PageLoadEndEventArgs(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {
        Browser = browser;
        Frame = frame;
        HttpStatusCode = httpStatusCode;
    }
}
