// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.Forms.@base;

public sealed class WindowStyleBuilder
{
    public WinFormium.Formium FormiumInstance { get; }

    public Control? ContainerControl { get; set; }

    internal WindowStyleBuilder(WinFormium.Formium formium, Control? containerControl = null)
    {
        FormiumInstance = formium;
        ContainerControl = containerControl;
    }
}

