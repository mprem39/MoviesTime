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

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest createMovieRequest)
        {
            var movie = createMovieRequest.MapToMovie();
            var created = await _movies.CreateAsync(movie);
            if (created)
            {
                var movieResponse = movie.MapToMovieResponse();
                //return Created($"/{ApiEndpoints.Movies.Create}/{movieResponse.Id}",  movieResponse);
                return CreatedAtAction(nameof(GetMovieById), new { idorSlug = movieResponse.Id }, movieResponse);
            }
            return BadRequest();
        }

        [HttpGet(ApiEndpoints.Movies.GetById)]
        public async Task<IActionResult> GetMovieById([FromRoute] string idorSlug)
        {
            var movie = Guid.TryParse(idorSlug, out var id) ? await _movies.GetByIdAsync(id) : await _movies.GetBySlugAsync(idorSlug);
            if (movie == null)
            {
                return NotFound();
            }
            var movieResponse = movie.MapToMovieResponse();
            return Ok(movieResponse);
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movies.GetAllAsync();
            var movieResponses = movies.MapToMoviesResponse();
            return Ok(movieResponses);
        }

        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest updateMovieRequest)
        {
            var movie = updateMovieRequest.MapToMovie(id);
            var updatedMovie = await _movies.UpdateAsync(movie);
            if (updatedMovie == null)
            {
                return NotFound();
            }
            var movieResponse = updatedMovie.MapToMovieResponse();
            return Ok(movieResponse);
        }

        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            var deleted = await _movies.DeleteByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
