// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WinFormium.Sources.Bootstrapper;

/// <summary>
/// 
/// </summary>
public sealed class MainWindowOptions
{
    internal ApplicationContext Context { get; set; } = new ApplicationContext();

    private IServiceCollection Services { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    internal MainWindowOptions(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    public MainWindowCreationAction UseMainForm(Form form)
    {
        Services.TryAddSingleton(form);

        return new MainWindowCreationAction(services =>
        {
            Context.MainForm = form;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public MainWindowCreationAction UseMainForm<T>() where T : Form
    {
        Services.TryAddSingleton<T>();

        return new MainWindowCreationAction(services =>
        {
            var form = services.GetRequiredService<T>();

            Context.MainForm = form;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="formium"></param>
    /// <returns></returns>
    public MainWindowCreationAction UseMainFormium(WinFormium.Formium formium)
    {
        Services.TryAddSingleton(formium);

        return new MainWindowCreationAction(services =>
        {
            Context.MainForm = formium.HostWindow;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    public MainWindowCreationAction UseMainFormium<T>(Func<T> configure) where T : WinFormium.Formium
    {
        Services.TryAddSingleton<T>();
        return new MainWindowCreationAction(services => Context.MainForm = configure.Invoke().HostWindow);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configure"></param>
    /// <returns></returns>
    public MainWindowCreationAction UseMainFormium<T>(Func<IServiceProvider, T> configure) where T : WinFormium.Formium
    {
        Services.TryAddSingleton<T>();
        return new MainWindowCreationAction(sp => Context.MainForm = configure.Invoke(sp).HostWindow);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public MainWindowCreationAction UseMainFormium<T>() where T : WinFormium.Formium
    {
        Services.TryAddSingleton<T>();

        return new MainWindowCreationAction(sp => Context.MainForm = sp.GetRequiredService<T>().HostWindow);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationContext"></param>
    /// <returns></returns>
    public MainWindowCreationAction UseWithoutMainForm(ApplicationContext? applicationContext = null)
    {
        return new MainWindowCreationAction(services =>
        {
            if (applicationContext != null)
                Context = applicationContext;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="creationAction"></param>
    /// <returns></returns>
    public MainWindowCreationAction UseWithoutMainForm(Func<ApplicationContext?> creationAction)
    {
        return new MainWindowCreationAction(services =>
        {
            var applicationContext = creationAction.Invoke();
            if (applicationContext != null)
                Context = applicationContext;
        });
    }
}
