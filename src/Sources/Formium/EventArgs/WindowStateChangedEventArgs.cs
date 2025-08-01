﻿// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Formium.EventArgs;

public class WindowStateChangedEventArgs : System.EventArgs
{
    public WindowStateChangedEventArgs(FormiumWindowState state)
    {
        WindowState = state;
    }

    public FormiumWindowState WindowState { get; }
}