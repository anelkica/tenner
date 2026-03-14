using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tenner.Core.Models;

namespace Tenner.Avalonia.Services;

// input validation is done in Tenner.API btw

public class TennerClient(HttpClient http) : ITennerClient
{
    private const string BaseUrl = "http://localhost:3333";

    public async Task<TenorResponse?> SearchAsync(string query, int limit = 20, string? pos = null)
    {
        var url = $"{BaseUrl}/api/gif/search?query={Uri.EscapeDataString(query)}&limit={limit}";
        if (pos is not null)
            url += $"&pos={Uri.EscapeDataString(pos)}";
        return await GetResponseAsync<TenorResponse>(url);
    }

    public async Task<TenorResponse?> GetFeaturedAsync(int limit = 20, string? pos = null)
    {
        var url = $"{BaseUrl}/api/gif/featured?limit={limit}";
        if (pos is not null)
            url += $"&pos={Uri.EscapeDataString(pos)}";
        return await GetResponseAsync<TenorResponse>(url);
    }

    public async Task<TenorSuggestionsResponse?> GetAutocompleteAsync(string query, int limit = 3)
    {
        var url = $"{BaseUrl}/api/gif/autocomplete?query={Uri.EscapeDataString(query)}&limit={limit}";
        return await GetResponseAsync<TenorSuggestionsResponse>(url);
    }

    public async Task RegisterShareAsync(string id, string? query = null)
    {
        var url = $"{BaseUrl}/api/gif/share?id={Uri.EscapeDataString(id)}";
        if (query is not null)
            url += $"&query={Uri.EscapeDataString(query)}";

        try
        {
            await http.PostAsync(url, null);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Share registration failed: {e.Message}");
        }
    }

    private async Task<T?> GetResponseAsync<T>(string url)
    {
        try
        {
            return await http.GetFromJsonAsync<T>(url);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Request failed for {url}: {e.Message}");
            return default;
        }
    }
}
