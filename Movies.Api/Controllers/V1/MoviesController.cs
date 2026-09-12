using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Appilication.Models;
using Movies.Application.Services;
using Movies.Contract.Requests.V1;
using Movies.Contract.Responses;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers.V1;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movies;

    public MoviesController(IMovieService movies)
    {
        _movies = movies;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.V1.Movies.Create)]
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
    
    [HttpGet(ApiEndpoints.V1.Movies.GetById)]
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
        movieResponse.Links.Add(new Link
        {
            Href = linkGenerator.GetUriByAction(HttpContext, nameof(GetMovieById), values: new {idorSlug = movie.Id}),
            Rel = "self",
            Type = "GET"
        });
        movieResponse.Links.Add(new Link
        {
            Href = linkGenerator.GetUriByAction(HttpContext, nameof(UpdateMovie), values: new { id = movie.Id }),
            Rel = "self",
            Type = "PUT"
        });
        movieResponse.Links.Add(new Link
        {
            Href = linkGenerator.GetUriByAction(HttpContext, nameof(DeleteMovie), values: new { id = movie.Id }),
            Rel = "self",
            Type = "DELETE"
        });
        return Ok(movieResponse);
    }
   
    [HttpGet(ApiEndpoints.V1.Movies.GetAll)]
    public async Task<IActionResult> GetAllMovies([FromQuery] GetAllMoviesRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions()
            .WithUser(userId);
        var movies = await _movies.GetAllAsync(options, token);
        var movieCount = await _movies.GetCountAsync(options.Title,options.YearOfRelease, token);
        var movieResponses = movies.MapToMoviesResponse(request.Page,request.PageSize,movieCount);
        return Ok(movieResponses);
    }
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.V1.Movies.Update)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest updateMovieRequest, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = updateMovieRequest.MapToMovie(id);
        var updatedMovie = await _movies.UpdateAsync(movie, userId, token);
        if (updatedMovie == null)
        {
            return NotFound();
        }
        var movieResponse = updatedMovie.MapToMovieResponse();
        return Ok(movieResponse);
    }
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.V1.Movies.Delete)]
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
