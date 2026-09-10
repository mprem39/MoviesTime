using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Appilication.Models;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movies;

        public MoviesController(IMovieService movies)
        {
            _movies = movies;
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest createMovieRequest, CancellationToken token)
        {
            var movie = createMovieRequest.MapToMovie();
            var created = await _movies.CreateAsync(movie, token);
            if (created)
            {
                var movieResponse = movie.MapToMovieResponse();
                //return Created($"/{ApiEndpoints.Movies.Create}/{movieResponse.Id}",  movieResponse);
                return CreatedAtAction(nameof(GetMovieById), new { idorSlug = movieResponse.Id }, movieResponse);
            }
            return BadRequest();
        }
        
        [HttpGet(ApiEndpoints.Movies.GetById)]
        public async Task<IActionResult> GetMovieById([FromRoute] string idorSlug, CancellationToken token)
        {
            var movie = Guid.TryParse(idorSlug, out var id) ? await _movies.GetByIdAsync(id, token) : await _movies.GetBySlugAsync(idorSlug, token);
            if (movie == null)
            {
                return NotFound();
            }
            var movieResponse = movie.MapToMovieResponse();
            return Ok(movieResponse);
        }
       
        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAllMovies(CancellationToken token)
        {
            var movies = await _movies.GetAllAsync(token);
            var movieResponses = movies.MapToMoviesResponse();
            return Ok(movieResponses);
        }
        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest updateMovieRequest, CancellationToken token)
        {
            var movie = updateMovieRequest.MapToMovie(id);
            var updatedMovie = await _movies.UpdateAsync(movie, token);
            if (updatedMovie == null)
            {
                return NotFound();
            }
            var movieResponse = updatedMovie.MapToMovieResponse();
            return Ok(movieResponse);
        }
        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _movies.DeleteByIdAsync(id, token);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
