using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Contract.Requests;

public class PagedRequest
{
    public required int Page { get; init; } = 1;
    public required int PageSize { get; init; } = 10;
}
