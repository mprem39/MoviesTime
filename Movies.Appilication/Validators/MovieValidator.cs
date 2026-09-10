using FluentValidation;
using Movies.Appilication.Models;
using Movies.Appilication.Repositories;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMoviesRepository _moviesRepository;
    public MovieValidator(IMoviesRepository moviesRepository)
    {
        _moviesRepository = moviesRepository;
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Genres)
            .NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty();
        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.Now.Year);
        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists in the system");

    }

    private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token = default)
    {
        var existingMovie = await _moviesRepository.GetBySlugAsync(slug);
        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;

        }
        return existingMovie is null;
    }
}
