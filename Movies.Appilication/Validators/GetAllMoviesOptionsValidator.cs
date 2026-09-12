using FluentValidation;
using Movies.Appilication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Validators;

public class GetAllMoviesOptionsValidator: AbstractValidator<GetAllMoviesOptions>
{
    private static readonly string[] AllowedSortFields = 
    {
        "title", "yearofrelease"
    };
    public GetAllMoviesOptionsValidator()
    {

        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.SortField)
            .Must(x => x is null || AllowedSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("You can only sort by title or yearofrelease");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 25)
            .WithMessage("PageSize must be between 1 and 25");

    }

}
