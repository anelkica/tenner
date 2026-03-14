using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Tenner.API.Configuration;
using Tenner.API.Interfaces;
using Tenner.Core.Models;

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

        return await GetResponseAsync<TenorResponse?>(url);
    }

    public async Task<TenorResponse?> GetFeaturedAsync(int limit = 10, string? pos = null)
    {
        string url = BuildUrl("featured", new Dictionary<string, string?>
        {
            ["limit"] = limit.ToString(),
            ["pos"] = pos
        });

        return await GetResponseAsync<TenorResponse?>(url);
    }

    public async Task<TenorSuggestionsResponse?> GetAutocompleteAsync(string query, int limit = 3)
    {
        string url = BuildUrl("autocomplete", new Dictionary<string, string?>
        {
            ["q"] = query,
            ["limit"] = limit.ToString()
        });

        return await GetResponseAsync<TenorSuggestionsResponse?>(url);
    }

    public async Task RegisterShareAsync(string id, string? query = null)
    {
        string url = BuildUrl("registershare", new Dictionary<string, string?>
        {
            ["id"] = id,
            ["q"] = query
        });

        try
        {
            await http.GetAsync(url); // fire & forget
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Failed to register share for GIF: {Id}", id);
        }
    }

    // prefer to use with TenorResponse or TenorSuggestionsResponse
    private async Task<T?> GetResponseAsync<T>(string url)
    {
        try
        {
            return await http.GetFromJsonAsync<T>(url);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Tenor API request failed for URL: {Url}", url);
            return default;
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
