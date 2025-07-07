// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.EventArgs;

public class FramePageLoadErrorEventArgs : System.EventArgs
{
    public CefBrowser Browser { get; }
    public CefFrame Frame { get; }
    public CefErrorCode ErrorCode { get; }
    public string ErrorText { get; }
    public string FailedUrl { get; }

    public FramePageLoadErrorEventArgs(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
    {
        Browser = browser;
        Frame = frame;
        ErrorCode = errorCode;
        ErrorText = errorText;
        FailedUrl = failedUrl;
    }
}
