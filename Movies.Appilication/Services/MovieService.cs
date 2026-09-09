

using Movies.Appilication.Models;
using Movies.Appilication.Repositories;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMoviesRepository _movieRepository;


    public MovieService(IMoviesRepository movieRepository)
    {
        _movieRepository = movieRepository;

    }

    public async Task<bool> CreateAsync(Movie movie)
    {
       
        return await _movieRepository.CreateAsync(movie);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        return _movieRepository.GetByIdAsync(id);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        return _movieRepository.GetBySlugAsync(slug);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return _movieRepository.GetAllMoviesAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        
        var movieExists = await _movieRepository.ExistbyId(movie.Id);
        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie);
        return movie;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _movieRepository.DeleteByIdAsync(id);
    }
}
