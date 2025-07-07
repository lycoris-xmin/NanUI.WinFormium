// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

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
        Services.AddSingleton(form);

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
        Services.AddSingleton<T>();

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
        Services.AddSingleton(formium);

        return new MainWindowCreationAction(services =>
        {
            Context.MainForm = formium.HostWindow;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public MainWindowCreationAction UseMainFormium<T>() where T : WinFormium.Formium
    {
        Services.AddSingleton<T>();

        return new MainWindowCreationAction(sp =>
        {
            var formium = sp.GetRequiredService<T>();
            Context.MainForm = formium.HostWindow;
        });
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

            //Context.MainForm = null;
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
            {
                Context = applicationContext;
            }
        });
    }
}
