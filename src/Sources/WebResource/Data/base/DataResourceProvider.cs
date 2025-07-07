// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI


// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.WebResource.@base;
using WinFormium.Sources.WebResource.Data.Attributes;

namespace WinFormium.Sources.WebResource.Data.@base;
public sealed class DataResourceProvider
{
    /// <summary>
    /// 
    /// </summary>
    public string ServiceSuffix { get; set; } = "ApiService";

    static Dictionary<string, DataResourceProvider> RegisteredProviders { get; } = new();

    private readonly List<DataResourceRoute> _routes = new();

    internal IEnumerable<DataResourceRoute> Routes => _routes;

    internal DataResourceOptions Options { get; }

    private DataResourceProvider(DataResourceOptions options)
    {
        Options = options;
    }

    internal static DataResourceProvider Create(DataResourceOptions options)
    {
        var domainName = GetUrlFromOptions(options);

        if (string.IsNullOrEmpty(domainName))
        {
            throw new ArgumentNullException(nameof(domainName), $"Argument must not be null.");
        }

        if (RegisteredProviders.ContainsKey(domainName))
        {
            throw new ArgumentException($"Domain name '{domainName}' is already registered.");
        }

        var provider = new DataResourceProvider(options);

        RegisteredProviders[domainName] = provider;

        return provider;
    }

    private static string GetUrlFromOptions(DataResourceOptions options)
    {
        return $"{options.Scheme}://{options.DomainName}".ToLower();
    }

    internal static DataResourceProvider? GetProvider(DataResourceOptions options)
    {
        var domainName = GetUrlFromOptions(options);

        if (!RegisteredProviders.ContainsKey(domainName)) return null;

        return RegisteredProviders[domainName];
    }

    public JsonSerializerOptions? DefaultJsonSerializerOptions { get; set; }

    internal static void RemoveProvider(DataResourceOptions options)
    {
        var domainName = GetUrlFromOptions(options);


        if (!RegisteredProviders.ContainsKey(domainName)) return;

        RegisteredProviders.Remove(domainName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    public void ImportFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            var assembly = Assembly.LoadFrom(fileName);
            ImportFromAssembly(assembly);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void ImportFromCurrentAssembly()
    {
        var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Entry assembly is null.");

        ImportFromAssembly(assembly);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void ImportFromAssembly(Assembly assembly)
    {
        if (assembly != null)
        {
            var services = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DataResourceService)));

            foreach (var service in services)
            {
                ImportDataResourceService(service);
            }
        }
        else
        {
            throw new ArgumentNullException($"{nameof(assembly)}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    public void ImportFromAssemblies(string[] files)
    {
        foreach (var fileName in files)
        {
            ImportFromFile(fileName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryName"></param>
    public void ImportFromDirectory(string directoryName)
    {
        if (Directory.Exists(directoryName))
        {
            foreach (var fileName in Directory.EnumerateFiles(directoryName, "*.dll"))
            {
                ImportFromFile(fileName);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ImportDataResourceService(Type type)
    {
        if (!type.IsSubclassOf(typeof(DataResourceService)))
        {
            throw new ArgumentNullException(nameof(type), $"Argument must not be null.");
        }

        var basePaths = new List<string>();

        var serviceName = type.Name;

        if (type.Name.EndsWith(ServiceSuffix))
            serviceName = serviceName.Substring(0, serviceName.LastIndexOf(ServiceSuffix));

        var routePathAttributes = type.GetCustomAttributes(typeof(RoutePathAttribute), false) as RoutePathAttribute[];

        if (routePathAttributes != null && routePathAttributes.Length > 0)
        {
            foreach (var routePathAttr in routePathAttributes)
            {
                serviceName = routePathAttr.Path.Replace(@"\", @"/"); ;
                serviceName = serviceName.Trim('/');

                basePaths.Add(serviceName);
            }
        }
        else
        {
            basePaths.Add(serviceName);
        }

        basePaths = [.. basePaths.Distinct()];

        var serviceActions = type.GetMethods().Where(x => x.ReturnType.IsAssignableFrom(typeof(IResourceResult)) || x.ReturnType.IsAssignableFrom(typeof(Task<IResourceResult>)));


        foreach (var serviceAction in serviceActions)
        {
            if (serviceAction.GetCustomAttributes(typeof(NoActionAttribute), true) is NoActionAttribute[] noActionAttributes && noActionAttributes.Length > 0)
                continue;


            if (serviceAction.GetCustomAttributes(typeof(HttpMethodAttribute), true) is HttpMethodAttribute[] actionHttpMethodAttributes)
            {
                foreach (var httpMethodAttr in actionHttpMethodAttributes)
                {
                    var path = string.Empty;
                    var method = ResourceRequestMethod.All;

                    if (httpMethodAttr == null)
                    {
                        path = serviceAction.Name;
                    }
                    else
                    {
                        method = httpMethodAttr.Method;

                        if (string.IsNullOrEmpty(httpMethodAttr.Path))
                        {
                            path = serviceAction.Name;
                        }
                        else
                        {
                            path = httpMethodAttr.Path!.Replace(@"\", @"/");
                            path = path.TrimEnd('/');
                        }
                    }

                    var paths = new List<string>();

                    if (path.StartsWith("/"))
                    {
                        paths.Add(path);
                    }
                    else
                    {
                        foreach (var basePath in basePaths)
                        {
                            paths.Add($"{basePath}/{path}");
                        }
                    }

                    paths = [.. paths.Select(x => x.ToLower()).Distinct()];

                    var route = new DataResourceRoute(method, [.. paths], serviceAction, DefaultJsonSerializerOptions);

                    _routes.Add(route);
                }
            }
            else
            {
                var path = serviceAction.Name;

                var paths = new List<string>();


                foreach (var basePath in basePaths)
                {
                    paths.Add($"{basePath}/{path}");
                }

                paths = [.. paths.Select(x => x.ToLower()).Distinct()];

                var route = new DataResourceRoute(ResourceRequestMethod.All, [.. paths], serviceAction, DefaultJsonSerializerOptions);

                _routes.Add(route);
            }
        }
    }
}
