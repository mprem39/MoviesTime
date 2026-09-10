using Movies.Appilication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication.Repositories
{
    public interface IMoviesRepository
    {
        Task<bool> CreateAsync(Movie movie, CancellationToken token=default);
        Task<Movie?> GetByIdAsync(Guid id, Guid? userId=default, CancellationToken token=default);
        Task<Movie?> GetBySlugAsync(string slug, Guid? userId=default, CancellationToken token=default);
        Task<IEnumerable<Movie>> GetAllMoviesAsync(Guid? userId, CancellationToken token=default);
        Task<bool> UpdateAsync(Movie movie, Guid? userId=default, CancellationToken token=default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token=default);
        Task<bool> ExistbyId(Guid id);
    }
}
