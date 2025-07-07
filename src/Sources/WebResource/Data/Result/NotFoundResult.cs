// THIS FILE IS PART OF WinFormium PROJECT
// THE WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) Xuanchen Lin. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/XuanchenLin/NanUI

using WinFormium.Sources.WebResource.@base;

namespace WinFormium.Sources.WebResource.Data.Result;

public class NotFoundResult : StatusCodeResult
{
    private const int DefaultStatusCode = StatusCodes.Status404NotFound;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundResult"/> class.
    /// </summary>
    public NotFoundResult() : base(DefaultStatusCode)
    {
    }
}
