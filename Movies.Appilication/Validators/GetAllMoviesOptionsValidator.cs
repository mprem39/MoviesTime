using FluentValidation;
using Movies.Appilication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Validators;

public class GetAllMoviesOptionsValidator: AbstractValidator<GetAllMoviesOptions>
{
    public GetAllMoviesOptionsValidator()
    {

        RuleFor(x => x.Year)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }

}
