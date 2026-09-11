using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Contract.Requests;

public class GetAllMoviesRequest
{
    public required string? Title { get; init; }
    public required int? Year { get; init; }
}
