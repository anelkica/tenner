using Microsoft.AspNetCore.Mvc;
using Tenner.API.Interfaces;
using Tenner.Core.Models;

namespace Tenner.API.Controllers;

[Route("api/gif")]
[ApiController]
public class GifController(ICacheService cache, ITenorService tenor, ILogger<GifController> logger) : ControllerBase
{
    [HttpGet("search")]
    [EndpointDescription("Searches GIFs based on a query")]
    public async Task<ActionResult<TenorResponse>> Search(string query, int limit = 10, string? pos = null)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query cannot be empty");
        if (limit is < 1 or > 50)
            return BadRequest("Limit must be between 1 and 50");

        string cacheKey = $"search:{query}:{limit}:{pos}";

        TenorResponse? cached = await cache.GetAsync<TenorResponse>(cacheKey);
        if (cached is not null)
            return Ok(cached);

        TenorResponse? results = await tenor.SearchAsync(query, limit, pos);
        if (results is null) // 502 Bad Gateway: upstream Tenor API request failed
            return StatusCode(502, "Failed to get results from Tenor");

        await cache.SetAsync(cacheKey, results, TimeSpan.FromMinutes(5));
        return Ok(results);
    }

    [HttpGet("featured")]
    [EndpointDescription("Returns featured GIFs on Tenor homepage. Cached for 15min.")]
    public async Task<ActionResult<TenorResponse>> GetFeatured(int limit = 10, string? pos = null)
    {
        if (limit is < 1 or > 50)
            return BadRequest("Limit must be between 1 and 50");

        string cacheKey = $"featured:{limit}:{pos}";

        TenorResponse? cached = await cache.GetAsync<TenorResponse>(cacheKey);
        if (cached is not null)
            return Ok(cached);

        TenorResponse? results = await tenor.GetFeaturedAsync(limit, pos);
        if (results is null)
            return StatusCode(502, "Failed to get featured GIFs from Tenor");

        await cache.SetAsync(cacheKey, results, TimeSpan.FromHours(12)); // mock
        //await cache.SetAsync(cacheKey, results, TimeSpan.FromMinutes(15));
        return Ok(results);
    }

    [HttpGet("autocomplete")]
    [EndpointDescription("Returns autocomplete suggestions for a partial search query. Use client-side debouncing.")]
    public async Task<ActionResult<TenorSuggestionsResponse>> Autocomplete(string query, int limit = 3)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query cannot be empty");

        if (limit is < 1 or > 5)
            return BadRequest("Limit must be between 1 and 5");

        TenorSuggestionsResponse? results = await tenor.GetAutocompleteAsync(query, limit);
        if (results is null)
            return StatusCode(502, "Failed to get autocomplete for query");

        return Ok(results);
    }

    [HttpPost("share")]
    [EndpointDescription("Registers a share to the official Tenor API. Use when selecting or sharing a GIF.")]
    public async Task<ActionResult> RegisterShare(string id, string? query = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID cannot be empty.");

        await tenor.RegisterShareAsync(id, query);
        return NoContent();
    }
}
