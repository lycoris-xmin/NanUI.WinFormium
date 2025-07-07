// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.EventArgs;

public class DownloadUpdatedEventArgs : System.EventArgs
{

    public DownloadUpdatedEventArgs(CefBrowser browser, CefDownloadItem item, CefDownloadItemCallback callback)
    {
        Browser = browser;
        Item = item;
        Callback = callback;
    }

    public CefBrowser Browser { get; }
    public CefDownloadItem Item { get; }
    public CefDownloadItemCallback Callback { get; }
}
