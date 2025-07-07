// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.WebResource.@base;

namespace WinFormium.Sources.WebResource.Data.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public abstract class HttpMethodAttribute : Attribute
{
    public ResourceRequestMethod Method { get; init; }
    public string? Path { get; init; }

    public HttpMethodAttribute(ResourceRequestMethod method, string? path = null)
    {
        Method = method;
        Path = path;
    }
}

