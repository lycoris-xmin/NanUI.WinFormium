// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using System.Diagnostics;
using WinFormium.Properties;
using WinFormium.Sources.Bootstrapper;
using WinFormium.Sources.Browser;
using WinFormium.Sources.Logging;
using WinFormium.Sources.WebResource.@base;
using WinFormium.Sources.WebResource.EmbeddedFile;
using PropertyManager = WinFormium.Sources.Utils.PropertyManager;

namespace WinFormium.Sources;

/// <summary>
/// 
/// </summary>
public sealed class WinFormiumApp
{
    private const string COMMON_DATA_DIR_NAME = @"WinFormium";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static AppBuilder CreateBuilder()
    {
        if (Current != null)
        {
            throw new ApplicationException(Messages.Exception_WinFormiumInitializationFailed);
        }
        ;

        return new AppBuilder();
    }

    /// <summary>
    /// 
    /// </summary>
    public static WinFormiumApp? Current { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public PlatformArchitecture Architecture { get; private set; }


    /// <summary>
    /// 
    /// </summary>
    public CultureInfo CurrentCulture => CultureInfo.GetCultureInfo(Properties.GetValue<string>(nameof(AppBuilder.UseCulture)) ?? Application.CurrentCulture.Name);

    /// <summary>
    /// 
    /// </summary>
    public ProcessType ProcessType { get; private set; }

    internal WinFormiumLogger Logger => ServiceProvider.GetRequiredService<WinFormiumLogger>();

    /// <summary>
    /// 
    /// </summary>
    public readonly IServiceProvider ServiceProvider;

    /// <summary>
    /// 
    /// </summary>
    public PropertyManager Properties => ServiceProvider.GetRequiredService<PropertyManager>();

    internal bool EnableDevTools => Properties.GetValue<bool>(nameof(AppBuilder.UseDevToolsMenu));

    internal string CommonDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), COMMON_DATA_DIR_NAME);

    internal string DefaultAppDataDirectory => Path.Combine(CommonDataDirectory, AppName);

    internal string AppDataDirectory { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string AppName { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public int BrowserProcessId { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private ChromiumEnvironment _env => ServiceProvider.GetRequiredService<ChromiumEnvironment>();

    /// <summary>
    /// 
    /// </summary>
    private IWinFormiumStartup _startup => ServiceProvider.GetRequiredService<IWinFormiumStartup>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processType"></param>
    /// <param name="architecture"></param>
    /// <param name="appName"></param>
    /// <param name="provider"></param>
    /// <exception cref="ApplicationException"></exception>
    internal WinFormiumApp(ProcessType processType, PlatformArchitecture architecture, IServiceProvider provider, string? appName = null)
    {
        if (Current != null)
        {
            // WinFormiumApp 已经初始化，只允许运行一个实例。
            throw new ApplicationException(Messages.Exception_WinFormiumInitializationFailed);
        }

        Current = this;

        ServiceProvider = provider;
        ProcessType = processType;
        Architecture = architecture;
        AppName = appName ?? Application.ProductName ?? "WinFormium App";

        AppDataDirectory = DefaultAppDataDirectory;

        DefaultLogger defaultLogger = new DefaultLogger { Log = new WinFormiumLogger(Path.Combine(AppDataDirectory, $"{nameof(WinFormium).ToLower()}.log"), true) };

        if (processType == ProcessType.Renderer)
        {
            var args = Environment.GetCommandLineArgs();

            var processIdArg = args.FirstOrDefault(x => x.StartsWith("--host-process-id")) ?? string.Empty;

            BrowserProcessId = int.Parse(Regex.Replace(processIdArg, "--host-process-id=", string.Empty));

            if (BrowserProcessId == 0)
                throw new ApplicationException(Messages.Exception_WinFormiumInitializationFailed);
        }
        else
        {
            BrowserProcessId = Process.GetCurrentProcess().Id;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private CefSettings GetDefaultCefSettings()
    {
        return new CefSettings
        {
            LogSeverity = CefLogSeverity.Error,
            ResourcesDirPath = _env.ResourceFilePath,
            LocalesDirPath = _env.LocaleFilePath,
            Locale = CurrentCulture.ToString(),
            JavaScriptFlags = "--expose-gc",
            UserDataPath = Path.Combine(AppDataDirectory, "User Data"),
            RootCachePath = AppDataDirectory,
            CachePath = _env.UseInMemoryUserData ? string.Empty : Path.Combine(AppDataDirectory, "Cache"),
            LogFile = Path.Combine(AppDataDirectory, $"{nameof(WinFormium).ToLower()}-cef.log"),
            MultiThreadedMessageLoop = true,
            ExternalMessagePump = false,
            NoSandbox = true,
            PersistSessionCookies = true,
            PersistUserPreferences = true,
            WindowlessRenderingEnabled = true,
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <param name="app"></param>
    /// <param name="exitCode"></param>
    /// <returns></returns>
    private bool Load(CefMainArgs args, CefApp app, [Optional] out int exitCode)
    {
        if (ProcessType == ProcessType.Main)
        {
            var useSingleton = Properties.GetValue<Action<OnApplicationInstanceRunningHandler>>(nameof(AppBuilder.UseSingleApplicationInstanceMode));

            if (useSingleton != null)
            {
                var thisProcess = Process.GetCurrentProcess();

                foreach (var process in Process.GetProcessesByName(thisProcess.ProcessName))
                {
                    if (process.Id != thisProcess.Id && process.HandleCount > 0)
                    {
                        useSingleton?.Invoke(new OnApplicationInstanceRunningHandler(process.Id));

                        Environment.Exit(0);
                        exitCode = 0;
                        return false;
                    }
                }
            }
        }

        if (_env.UserDataPath != null && !Directory.Exists(_env.UserDataPath))
        {
            var userDataDir = _env.UserDataPath;

            try
            {
                Directory.CreateDirectory(userDataDir);

                AppDataDirectory = userDataDir;
            }
            catch
            {
                AppDataDirectory = DefaultAppDataDirectory;
            }
        }

        try
        {
            Application.CurrentCulture = CurrentCulture;
            CultureInfo.DefaultThreadCurrentCulture = CurrentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CurrentCulture;

            var shouldClearCache = Properties.GetValue<bool?>(nameof(AppBuilder.ClearCache)) ?? false;

            if (shouldClearCache)
            {
                try
                {
                    if (Directory.Exists(AppDataDirectory))
                    {
                        Directory.Delete(AppDataDirectory, true);
                    }
                }
                catch (IOException)
                {

                }
            }

            CefRuntime.Load(_env.LibCefPath);

            if (!_env.DisableHiDpiSupport)
            {
                CefRuntime.EnableHighDpiSupport();
            }



            exitCode = CefRuntime.ExecuteProcess(args, app, nint.Zero);


            if (exitCode != -1)
            {
                Environment.Exit(exitCode);

                Debug.WriteLine($"ExecuteProcess() expected to return -1 but returned {exitCode}");

                Logger.LogInformation($"ExecuteProcess() expected to return -1 but returned {exitCode}");

                return false;
            }

            foreach (var arg in Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("--type="))
                {

                    exitCode = -2;
                    Environment.Exit(exitCode);

                    Debug.WriteLine($"ExecuteProcess() expected to return -1 but returned {exitCode}");

                    Logger.LogInformation($"ExecuteProcess() expected to return -1 but returned {exitCode}");

                    return false;
                }
            }


            Debug.WriteLine($"ExecuteProcess() returns {exitCode}");

            Logger.LogInformation($"ExecuteProcess() returns {exitCode}");
        }
        catch (CefVersionMismatchException ex)
        {
            var title = Messages.Exception_LibCefLoadFailed_Title;
            var msg = Messages.Exception_LibCefLoadFailed_NotFoundOrArchError;

            Logger.LogError(ex);

            Debug.WriteLine(ex);

            MessageBox.Show($"{msg}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);

            exitCode = -2;

            return false;

        }
        catch (DllNotFoundException ex)
        {
            var title = Messages.Exception_LibCefLoadFailed_Title;
            var msg = Messages.Exception_LibCefLoadFailed_NotFoundOrArchError;

            Debug.WriteLine(ex);

            Logger.LogError(ex);


            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

            exitCode = -2;

            return false;

        }
#if DEBUG
        var buildType = "DEBUG";
#else
        var buildType = "RELEASE";
#endif
        var license = string.Format(Resources.License, DateTime.Now.Year);

        var copyrightInfo = $"""
* Welcome to WinFormium ({buildType}); Chromium Embedded/{CefRuntime.ChromeVersion}; WinFormium/{Assembly.GetExecutingAssembly().GetName().Version}_{Architecture};
Copyrights (C) 2012-{DateTime.Now.Year} Xuanchen Lin all rights reserved. Made in Kunming, China.
{Resources.ASCII}
This project is under MIT License.

https://github.com/XuanchenLin/WinFormium/blob/master/LICENCE

""";

        var info = $"""
{copyrightInfo}
{license}

""";


        Debug.WriteLine($"\n{info}");

        Console.WriteLine(info);



        return true;
    }

    //Resource.EmbeddedFileResourceSchemeHandlerFactory? InternalPagesResourceSchemeHandlerFactory { get; set; }
    //Resource.EmbeddedFileResourceSchemeHandlerFactory? AboutFormResourceSchemeHandlerFactory { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public void Run()
    {
        var args = new CefMainArgs(Environment.GetCommandLineArgs());

        var app = new BrowserApp(this);

        if (Load(args, app, out _))
        {
            var settings = GetDefaultCefSettings();

            _env.ConfigureSettings?.Invoke(settings);

            var subprocessRetval = ConfigureSubprcess(settings);

            if (subprocessRetval == false)
            {
                MessageBox.Show(Messages.WinFormiumApp_Subprocess_NotFound_Text, Messages.WinFormiumApp_Subprocess_NotFound_Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Shutdown();

                Environment.Exit(-2);

                return;
            }

            CefRuntime.Initialize(args, settings, app, nint.Zero);

            //_startup.WinFormiumMain(Environment.GetCommandLineArgs());

            var resourceSchemeHandlerFactories = ServiceProvider.GetServices<ResourceSchemeHandlerFactory>();

            foreach (var factory in resourceSchemeHandlerFactories)
            {
                factory.ResourceSchemeHandlerRegister(ServiceProvider);

                CefRuntime.RegisterSchemeHandlerFactory(factory.Scheme, factory.DomainName, factory);
            }

            var internalPagesResourceSchemeHandlerFactory = new EmbeddedFileResourceSchemeHandlerFactory(new EmbeddedFileResourceOptions
            {
                Scheme = "formium",
                DomainName = "pages",
                ResourceAssembly = typeof(WinFormiumApp).Assembly,
                OnFallback = (url) => "/index.html",
                EmbeddedResourceDirectoryName = "Resources/WebRoot/InternalPages",
                DefaultNamespace = "WinFormium.Ultimate"
            });

            var aboutFormResourceSchemeHandlerFactory = new EmbeddedFileResourceSchemeHandlerFactory(new EmbeddedFileResourceOptions
            {
                Scheme = "formium",
                DomainName = "about",
                ResourceAssembly = typeof(WinFormiumApp).Assembly,
                OnFallback = (url) => "/index.html",
                EmbeddedResourceDirectoryName = "Resources/WebRoot/AboutForm",
                DefaultNamespace = "WinFormium.Ultimate"
            });

            CefRuntime.RegisterSchemeHandlerFactory(internalPagesResourceSchemeHandlerFactory.Scheme, internalPagesResourceSchemeHandlerFactory.DomainName, internalPagesResourceSchemeHandlerFactory);

            CefRuntime.RegisterSchemeHandlerFactory(aboutFormResourceSchemeHandlerFactory.Scheme, aboutFormResourceSchemeHandlerFactory.DomainName, aboutFormResourceSchemeHandlerFactory);

            //var appContext = new ApplicationContext();
            //var createMainWindowOpts = new MainWindowOptions(appContext);

            try
            {
                var createMainWindowAction = ServiceProvider.GetRequiredService<MainWindowCreationAction>();

                var mainWindowOptions = ServiceProvider.GetRequiredService<MainWindowOptions>();

                createMainWindowAction.Invoke(ServiceProvider);

                createMainWindowAction.Dispose();

                Application.Run(mainWindowOptions.Context);
            }
            finally
            {
                CefRuntime.Shutdown();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void RunWithoutContext()
    {
        var args = new CefMainArgs(Environment.GetCommandLineArgs());

        var app = new BrowserApp(this);

        if (Load(args, app, out _))
        {
            var settings = GetDefaultCefSettings();

            _env.ConfigureSettings?.Invoke(settings);

            var subprocessRetval = ConfigureSubprcess(settings);

            if (subprocessRetval == false)
            {
                MessageBox.Show(Messages.WinFormiumApp_Subprocess_NotFound_Text, Messages.WinFormiumApp_Subprocess_NotFound_Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Shutdown();

                Environment.Exit(-2);

                return;
            }

            CefRuntime.Initialize(args, settings, app, nint.Zero);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void RunAsSubprocess()
    {
        var args = new CefMainArgs(Environment.GetCommandLineArgs());

        var app = new BrowserApp(this);

        if (Load(args, app, out var exitCode))
        {
            Environment.Exit(exitCode);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    private bool ConfigureSubprcess(CefSettings settings)
    {
        if (_env.ConfigureSubprocess != null)
        {
            var options = new SubprocessOptions { Architecture = _env.Architecture };
            _env.ConfigureSubprocess.Invoke(options);

            if (options.SubprocessFilePath != null && File.Exists(options.SubprocessFilePath))
            {
                var fileInfo = new FileInfo(options.SubprocessFilePath);

                settings.BrowserSubprocessPath = fileInfo.FullName;

                return true;
            }
            else
            {
                var retval = options.SubprocessNotExists?.Invoke(_env.Architecture, options.SubprocessFilePath) ?? false;


                return retval;
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void Shutdown()
    {

        CefRuntime.Shutdown();
    }
}
