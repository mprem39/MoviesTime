using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping.V2;
using Movies.Appilication.Models;
using Movies.Application.Services;
using Movies.Contract.Requests.V2;
using Movies.Contract.Responses;

namespace Movies.Api.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movies;

    public MoviesController(IMovieService movies)
    {
        _movies = movies;
    }

    
    [HttpGet(ApiEndpoints.Movies.GetById)]
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

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAllMoviesV1([FromQuery] GetAllMoviesRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions()
            .WithUser(userId);
        var movies = await _movies.GetAllAsync(options, token);
        var movieCount = await _movies.GetCountAsync(options.Title, options.YearOfRelease, token);
        var movieResponses = movies.MapToMoviesResponse(request.Page, request.PageSize, movieCount);
        return Ok(movieResponses);
    }


}
