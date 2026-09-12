using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Appilication.Models;
using Movies.Application.Services;

using Movies.Contract.Responses;

namespace Movies.Api.Controllers.V2;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movies;

    public MoviesController(IMovieService movies)
    {
        _movies = movies;
    }

    
    [HttpGet(ApiEndpoints.V2.Movies.GetById)]
    public async Task<IActionResult> GetMovieById([FromRoute] string idorSlug,[FromServices] LinkGenerator linkGenerator,CancellationToken token)
    {
        var userId= HttpContext.GetUserId();
        var movie = Guid.TryParse(idorSlug, out var id) ? await _movies.GetByIdAsync(id, userId, token) : await _movies.GetBySlugAsync(idorSlug, userId,token);
        if (movie == null)
        {
            return NotFound();
        }
        var movieResponse = movie.MapToMovieResponse();
        var movieObject = new { id = movie.Id };

        return Ok(movieResponse);
    }
   
    
}
