// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.Logging;
using PropertyManager = WinFormium.Sources.Utils.PropertyManager;

namespace WinFormium.Sources.Bootstrapper;

/// <summary>
/// 
/// </summary>
public sealed class AppBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public IServiceCollection Services { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public PropertyManager Properties { get; }

    /// <summary>
    /// 
    /// </summary>
    public ProcessType ProcessType { get; }

    private const string TYPE_ARG_PREFIX = "--type=";
    internal const string SETTINGS_ENABLE_EMBEDDED_BROWSER = "EnableEmbeddedBrowser";

    private Action<ChromiumEnvironmentBuiler>? _configureChromium;

    internal List<Action<AppBuilder>> UseExtensions { get; } = new();

    internal AppBuilder()
    {
        Services = new ServiceCollection();

        Properties = new PropertyManager();

        var args = Environment.GetCommandLineArgs();

        ProcessType = args.FirstOrDefault(x => x.StartsWith(TYPE_ARG_PREFIX)) == null ? ProcessType.Main : ProcessType.Renderer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void UserServiceCollection(IServiceCollection services)
    {
        Services = services;
        Services.AddSingleton(Properties);
    }

    internal PlatformArchitecture Architecture
    {
        get
        {
            return nint.Size switch
            {
                4 => PlatformArchitecture.x86,
                8 => PlatformArchitecture.x64,
                _ => throw new NotSupportedException(WinFormium.Properties.Messages.Exception_UnkownPlatformArchitecture),
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configureDelegate"></param>
    /// <returns></returns>
    public AppBuilder ConfigureChromium(Action<ChromiumEnvironmentBuiler> configureDelegate)
    {
        _configureChromium += configureDelegate;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TApp"></typeparam>
    /// <returns></returns>
    public AppBuilder UseWinFormiumApp<TApp>() where TApp : notnull, WinFormiumStartup
    {
        Services.AddSingleton<IWinFormiumStartup, TApp>();

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    public AppBuilder UseCulture(string culture)
    {
        Configure(@this =>
        {
            @this.Properties.SetValue(nameof(UseCulture), culture);
        });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AppBuilder UseDevToolsMenu()
    {
        Configure(@this =>
        {
            @this.Properties.SetValue(nameof(UseDevToolsMenu), true);
        });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AppBuilder UseEmbeddedBrowser()
    {
        Configure(@this =>
        {
            @this.Properties.SetValue(nameof(UseEmbeddedBrowser), true);
        });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AppBuilder ClearCache()
    {
        Configure(@this =>
        {
            @this.Properties.SetValue(nameof(ClearCache), true);
        });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="onApplicationInstanceRunning"></param>
    /// <returns></returns>
    public AppBuilder UseSingleApplicationInstanceMode(Action<OnApplicationInstanceRunningHandler>? onApplicationInstanceRunning = null)
    {
        Configure(@this =>
        {
            @this.Properties.SetValue(nameof(UseSingleApplicationInstanceMode), onApplicationInstanceRunning ?? new Action<OnApplicationInstanceRunningHandler>(_ => { }));
        });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public AppBuilder UseServices(Action<IServiceCollection> action)
    {
        action.Invoke(Services);

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="useExtension"></param>
    /// <returns></returns>
    public AppBuilder Configure(Action<AppBuilder> useExtension)
    {
        UseExtensions.Add(useExtension);

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void Build()
    {
        foreach (var extension in UseExtensions)
        {
            extension.Invoke(this);
        }

        var chromiumConfig = ChromiumEnvironmentBuiler.Create(this);

        if (ProcessType == ProcessType.Main)
        {
            var tempServiceProvider = Services.BuildServiceProvider();

            var startup = tempServiceProvider.GetRequiredService<IWinFormiumStartup>();

            _configureChromium += startup.ConfigureChromiumEmbeddedFramework;

            startup.WinFormiumMain(Environment.GetCommandLineArgs());

            startup.ConfigureServices(Services);

            var mainWindowOpts = new MainWindowOptions(Services)!;

            Services.AddSingleton(mainWindowOpts);

            var createAction = startup.UseMainWindow(mainWindowOpts);

            if (createAction != null)
            {
                Services.AddSingleton(mainWindowOpts);

                Services.AddSingleton(createAction);
            }
        }

        _configureChromium?.Invoke(chromiumConfig);

        var env = chromiumConfig.Build();

        Services.AddSingleton(env);

        Services.AddSingleton(provider =>
        {
            return (WinFormiumLogger)Logger.Instance.Log;
        });

        Services.AddSingleton(sp =>
        {
            return new WinFormiumApp(ProcessType, Architecture, sp);
        });
    }
}
