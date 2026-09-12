using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Contract.Requests.V2;

public class GetAllMoviesRequest : PagedRequest
{
    public required string? Title { get; init; }
    public required int? YearOfRelease { get; init; }
    public required string? SortBy { get; init; }
}
