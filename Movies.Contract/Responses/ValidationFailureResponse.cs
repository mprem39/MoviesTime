using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Contract.Responses;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}

public class  ValidationResponse
{
    public required string PropertyName { get; init; }
    public required string Message { get; init; }
}