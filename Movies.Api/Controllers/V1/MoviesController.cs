using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping.V1;
using Movies.Appilication.Models;
using Movies.Application.Services;
using Movies.Contract.Requests.V1;
using Movies.Contract.Responses;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movies;
    private readonly IOutputCacheStore _outputCacheStore;

    public MoviesController(IMovieService movies, IOutputCacheStore outputCacheStore)
    {
        _movies = movies;
        _outputCacheStore = outputCacheStore;
    }

    //[Authorize(AuthConstants.TrustedMemberPolicyName)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [HttpPost(ApiEndpoints.Movies.Create)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest createMovieRequest, CancellationToken token)
    {
        var movie = createMovieRequest.MapToMovie();
        var created = await _movies.CreateAsync(movie, token);
       await _outputCacheStore.EvictByTagAsync("MovieCache", token);
        if (created)
        {
            var movieResponse = movie.MapToMovieResponse();
            //return Created($"/{ApiEndpoints.Movies.Create}/{movieResponse.Id}",  movieResponse);
            return CreatedAtAction(nameof(GetMovieById), new { idorSlug = movieResponse.Id }, movieResponse);
        }
        return BadRequest();
    }
    
    [HttpGet(ApiEndpoints.Movies.GetById)]
    [OutputCache(PolicyName = "MovieCache")]
    //[ResponseCache(Duration =30,VaryByHeader = "Accept , Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [OutputCache(PolicyName = "MovieCache")]
    //[ResponseCache(Duration = 30,VaryByQueryKeys = new string[] {"title","year","sortby","page", "pageSize" }, VaryByHeader = "Accept , Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [ProducesResponseType(typeof(PagedResponse<MovieResponse>), StatusCodes.Status200OK)]
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
    [HttpPut(ApiEndpoints.Movies.Update)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest updateMovieRequest, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = updateMovieRequest.MapToMovie(id);
        var updatedMovie = await _movies.UpdateAsync(movie, userId, token);
        if (updatedMovie == null)
        {
            return NotFound();
        }
        await _outputCacheStore.EvictByTagAsync("MovieCache", token);
        var movieResponse = updatedMovie.MapToMovieResponse();
        return Ok(movieResponse);
    }
    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _movies.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }
        await _outputCacheStore.EvictByTagAsync("MovieCache", token);
        return NoContent();
    }
}
