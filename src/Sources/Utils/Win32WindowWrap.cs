// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Utils;

internal class Win32WindowWrap : IWin32Window
{
    public nint Handle { get; }

    public Win32WindowWrap(nint handle)
    {
        Handle = handle;
    }
}
