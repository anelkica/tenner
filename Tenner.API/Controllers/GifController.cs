using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenner.API.Interfaces;
using Tenner.API.Models;

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
}
