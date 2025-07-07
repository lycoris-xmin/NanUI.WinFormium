// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.EventArgs;

public class PageLoadingProgressChangeEventArgs : System.EventArgs
{
    public PageLoadingProgressChangeEventArgs(CefBrowser browser, decimal progress)
    {
        Progress = progress;
    }

    public decimal Progress { get; }
}
