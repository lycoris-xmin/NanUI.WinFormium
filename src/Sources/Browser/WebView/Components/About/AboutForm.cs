// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.Formium.Forms;
using WinFormium.Sources.Formium.Forms.@base;

namespace WinFormium.Sources.Browser.WebView.Components.About;
internal class AboutForm : WinFormium.Formium
{
    public AboutForm()
    {
        Url = "formium:about";
        EnableSplashScreen = false;
    }

    protected override bool DisableAboutMenu => true;

    protected override FormStyle ConfigureWindowStyle(WindowStyleBuilder windowStyle)
    {
        var style = windowStyle.UseSystemForm();

        style.TitleBar = false;
        style.Sizable = false;
        style.Size = new Size(720, 500);
        style.StartCentered = StartCenteredMode.CenterParent;
        style.Maximizable = false;
        style.Minimizable = false;
        style.Icon = null;
        style.ShowInTaskbar = false;
        style.AllowFullScreen = false;
        style.AllowSystemMenu = false;

        return style;
    }

    protected override bool BeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
    {
        var ps = new System.Diagnostics.ProcessStartInfo(targetUrl)
        {
            UseShellExecute = true,
            Verb = "open"
        };
        System.Diagnostics.Process.Start(ps);

        return true;
    }
}
