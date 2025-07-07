// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.WebResource.@base;

namespace WinFormium.Sources.WebResource.Data.@base;

public sealed class DataResourceContext
{
    public required ResourceRequest Request { get; init; }
    public required ResourceResponse Response { get; init; }
    public required DataResourceRoute Route { get; init; }

    public DataResourceContext()
    {

    }
}
