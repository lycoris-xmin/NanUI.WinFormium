// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
//
// GITHUB: https://github.com/XuanchenLin/WinFormium
// EMail: xuanchenlin(at)msn.com QQ:19843266 WECHAT:linxuanchen1985



// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
//
// GITHUB: https://github.com/XuanchenLin/WinFormium
// EMail: xuanchenlin(at)msn.com QQ:19843266 WECHAT:linxuanchen1985


using WinFormium.Forms.SystemForm;
using WinFormium.Sources.Formium.Forms.@base;

namespace WinFormium.Sources.Formium.Forms;

/// <summary>
/// 
/// </summary>
public sealed class SystemFormStyle : FormStyle
{
    private bool _useDirectCompositon = false;

    /// <summary>
    /// 
    /// </summary>
    internal protected override bool UseBrowserHitTest { get; set; }

    internal Func<Bitmap?>? ShouldDrawSpalsh { get; set; }

    internal SystemFormStyle(WinFormium.Formium formium) : base(formium)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public bool TitleBar { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    protected internal override bool HasSystemTitleBar { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override CreateHostWindowDelegate CreateHostWindow()
    {
        return () =>
        {
            StandardWindowBase target;

            if (TitleBar)
            {
                UseBrowserHitTest = false;
                HasSystemTitleBar = true;
                target = new SystemStandardWindow(this);

            }
            else
            {
                UseBrowserHitTest = true;
                HasSystemTitleBar = false;
                target = new SystemBorderlessWindow(this);
            }

            return target;
        };
    }
}

/// <summary>
/// 
/// </summary>
public static class SystemFormStyleExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static SystemFormStyle UseSystemForm(this WindowStyleBuilder builder)
    {
        return new SystemFormStyle(builder.FormiumInstance);
    }
}

