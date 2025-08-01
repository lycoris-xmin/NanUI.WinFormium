// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.Browser.@base;

namespace WinFormium.Sources.Formium.BrowserHandlerImplements;
public abstract class CommandHandler : ICommandHandler
{
    bool ICommandHandler.OnChromeCommand(CefBrowser browser, int commandId, CefWindowOpenDisposition disposition)
    {
        return OnChromeCommand(browser, commandId, disposition);
    }

    protected virtual bool OnChromeCommand(CefBrowser browser, int commandId, CefWindowOpenDisposition disposition)
    {
        return false;
    }
}
