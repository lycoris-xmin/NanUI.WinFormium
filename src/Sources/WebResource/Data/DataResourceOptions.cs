// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.WebResource.@base;
using WinFormium.Sources.WebResource.Data.@base;

namespace WinFormium.Sources.WebResource.Data;
internal class DataResourceOptions : ResourceOptions
{
    public required Action<DataResourceProvider> ConfigureDataResourceProvider { get; init; }
}
