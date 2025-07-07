// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI


// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Properties;

namespace WinFormium.Sources.Formium.Forms.@base;

/// <summary>
/// 
/// </summary>
public abstract class FormStyle : IFormStyle
{
    WndProcDelegate? IFormStyle.OnWndProc { get => OnWndProc; set => OnWndProc = value; }
    WndProcDelegate? IFormStyle.OnDefWndProc { get => OnDefWndProc; set => OnDefWndProc = value; }
    OnOffscreenPaintDelegate? IFormStyle.OffscreenPaint { get => OnOffscreenPaint; set => OnOffscreenPaint = value; }

    /// <summary>
    /// 
    /// </summary>
    internal protected abstract bool HasSystemTitleBar { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal protected abstract bool UseBrowserHitTest { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal protected virtual bool AsEmbeddedControl { get; } = false;

    /// <summary>
    /// 
    /// </summary>
    internal protected WndProcDelegate? OnWndProc { internal get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal protected WndProcDelegate? OnDefWndProc { internal get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal protected OnOffscreenPaintDelegate? OnOffscreenPaint { internal get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected WinFormium.Formium FormiumInstance { get; }

    private Point? _location = null;
    /// <summary>
    /// 
    /// </summary>
    internal protected bool OffScreenRenderEnabled
    {
        get;
        protected set;
    } = false;

    /// <summary>
    /// 
    /// </summary>
    public Size Size { get; set; } = new Size(960, 640);

    /// <summary>
    /// 
    /// </summary>
    public Size MaximumSize { get; set; } = Size.Empty;

    /// <summary>
    /// 
    /// </summary>
    public Size MinimumSize { get; set; } = Size.Empty;

    /// <summary>
    /// 
    /// </summary>
    public Icon? Icon { get; set; } = Resources.DefaultIcon;

    /// <summary>
    /// 
    /// </summary>
    public string DefaultAppTitle { get; set; } = "WinFormium";

    /// <summary>
    /// 
    /// </summary>
    public bool UsePageTitle { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal protected bool IsLocationSet => _location != null;

    /// <summary>
    /// 
    /// </summary>
    public Point Location { get => _location ?? Point.Empty; set => _location = value; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowFullScreen { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool ShowInTaskbar { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public StartCenteredMode StartCentered { get; set; } = StartCenteredMode.None;

    /// <summary>
    /// 
    /// </summary>
    public bool Sizable { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool Maximizable { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool Minimizable { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public bool TopMost { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool AllowSystemMenu { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public FormiumColorMode ColorMode { get; set; } = FormiumColorMode.SystemPreference;

    /// <summary>
    /// 
    /// </summary>
    public FormiumWindowState WindowState { get; set; } = FormiumWindowState.Normal;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="formium"></param>
    public FormStyle(WinFormium.Formium formium)
    {
        FormiumInstance = formium;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract CreateHostWindowDelegate CreateHostWindow();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="form"></param>
    internal protected virtual void OverwriteHostWindowProperties(Form form)
    {

    }
}

/// <summary>
/// 
/// </summary>
public enum StartCenteredMode
{
    /// <summary>
    /// 
    /// </summary>
    None,
    /// <summary>
    /// 
    /// </summary>
    CenterScreen,
    /// <summary>
    /// 
    /// </summary>
    CenterParent
}