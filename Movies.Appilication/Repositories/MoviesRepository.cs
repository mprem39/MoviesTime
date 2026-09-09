using Movies.Appilication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private static readonly List<Movie> _movies = new();
        public Task<bool> CreateAsync(Movie movie)
        {
            _movies.Add(movie);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removeCount = _movies.RemoveAll(m => m.Id == id);
            var movieRemoved = removeCount > 0;
            return Task.FromResult(movieRemoved);
        }

        public Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return Task.FromResult<IEnumerable<Movie>>(_movies);
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            var movie = _movies.SingleOrDefault(m => m.Id == id);
            return Task.FromResult<Movie?>(movie);
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            var movie = _movies.SingleOrDefault(m => m.Slug == slug);
            return Task.FromResult<Movie?>(movie);
        }

        public Task<bool> UpdateAsync(Movie movie)
        {
            var movieIndex = _movies.FindIndex(m => m.Id == movie.Id);
            if (movieIndex == -1)
            {
                return Task.FromResult(false);
            }
            _movies[movieIndex] = movie;
            return Task.FromResult(true);
        }
    }
}
