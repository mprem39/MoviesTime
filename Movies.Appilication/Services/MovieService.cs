

using FluentValidation;
using Movies.Appilication.Models;
using Movies.Appilication.Repositories;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMoviesRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;


    public MovieService(IMoviesRepository movieRepository, IValidator<Movie> validator)
    {
        _movieRepository = movieRepository;
        _movieValidator = validator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);

        return await _movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return _movieRepository.GetByIdAsync(id, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token)
    {
        return _movieRepository.GetBySlugAsync(slug, token);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token)
    {
        return _movieRepository.GetAllMoviesAsync(token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await _movieRepository.ExistbyId(movie.Id);
        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie, token);
        return movie;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token)
    {
        return _movieRepository.DeleteByIdAsync(id, token);
    }
}
