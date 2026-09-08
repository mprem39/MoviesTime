using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Appilication.Models;
using Movies.Appilication.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesRepository _moviesRepository;

        public MoviesController(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest createMovieRequest)
        {
            var movie = createMovieRequest.MapToMovie();
            var created = await _moviesRepository.CreateAsync(movie);
            if (created)
            {
                var movieResponse = movie.MapToMovieResponse();
                //return Created($"/{ApiEndpoints.Movies.Create}/{movieResponse.Id}",  movieResponse);
                return CreatedAtAction(nameof(GetMovieById), new { id = movieResponse.Id }, movieResponse);
            }
            return BadRequest();
        }

        [HttpGet(ApiEndpoints.Movies.GetById)]
        public async Task<IActionResult> GetMovieById([FromRoute] Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);
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
            var movies = await _moviesRepository.GetAllMoviesAsync();
            var movieResponses = movies.MapToMoviesResponse();
            return Ok(movieResponses);
        }

        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest updateMovieRequest)
        {
            var movie = updateMovieRequest.MapToMovie(id);
            var updated = await _moviesRepository.UpdateAsync(movie);
            if (!updated)
            {
                return BadRequest();
            }
            var movieResponse = movie.MapToMovieResponse();
            return Ok(movieResponse);
        }

        [HttpDelete(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            var deleted = await _moviesRepository.DeleteByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
