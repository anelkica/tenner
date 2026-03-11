using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Tenner.API.Configuration;
using Tenner.API.Interfaces;
using Tenner.API.Models;

namespace Tenner.API.Services;

public class TenorService(HttpClient http, IOptions<TenorConfig> config, ILogger<TenorService> logger) : ITenorService
{
    private readonly TenorConfig _config = config.Value;
    private const string BaseUrl = "https://tenor.googleapis.com/v2";

    public async Task<TenorResponse?> SearchAsync(string query, int limit = 10, string? pos = null)
    {
        string url = BuildUrl("search", new Dictionary<string, string?>
        {
            ["q"] = query,
            ["limit"] = limit.ToString(),
            ["pos"] = pos
        });

        return await GetAsync(url);
    }

    private async Task<TenorResponse?> GetAsync(string url)
    {
        try
        {
            return await http.GetFromJsonAsync<TenorResponse>(url);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Tenor API request failed for URL: {Url}", url);
            return null;
        }
    }

    private string BuildUrl(string endpoint, Dictionary<string, string?> queryParameters)
    {
        // TenorConfig.cs, appsettings.json
        var query = new Dictionary<string, string?>
        {
            ["key"] = _config.ApiKey,
            ["client_key"] = _config.ClientKey,
            ["media_filter"] = _config.MediaFilter
        };

        foreach (var (k, v) in queryParameters)
            if (v is not null)
                query[k] = v;

        return QueryHelpers.AddQueryString($"{BaseUrl}/{endpoint}", query);
    }
}
