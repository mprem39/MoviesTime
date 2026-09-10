using Movies.Appilication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Repositories
{
    public interface IMoviesRepository
    {
        Task<bool> CreateAsync(Movie movie, CancellationToken token=default);
        Task<Movie?> GetByIdAsync(Guid id, CancellationToken token=default);
        Task<Movie?> GetBySlugAsync(string slug, CancellationToken token=default);
        Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken token=default);
        Task<bool> UpdateAsync(Movie movie, CancellationToken token=default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token=default);
        Task<bool> ExistbyId(Guid id);
    }
}
