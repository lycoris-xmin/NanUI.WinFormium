// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

namespace WinFormium.Sources.Bootstrapper;

/// <summary>
/// 
/// </summary>
public sealed class MainWindowCreationAction : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createAction"></param>
    public MainWindowCreationAction(Action<IServiceProvider> createAction)
    {
        CreateAction = createAction;
    }

    internal Action<IServiceProvider> CreateAction { get; }

    internal void Invoke(IServiceProvider services)
    {
        CreateAction.Invoke(services);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
    }
}


