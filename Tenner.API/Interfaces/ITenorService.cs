using Tenner.Core.Models;

namespace Tenner.API.Interfaces;

public interface ITenorService
{
    Task<TenorResponse?> SearchAsync(string query, int limit = 10, string? pos = null);
    Task<TenorResponse?> GetFeaturedAsync(int limit = 10, string? pos = null);
    Task<TenorSuggestionsResponse?> GetAutocompleteAsync(string query, int limit = 3);

    /// <summary>Registers GIF share event to the official Tenor API, for improving search results.</summary>
    /// <remarks>Call when user selects and shares GIF.</remarks>
    /// <param name="id">ID of the GIF</param>
    /// <param name="query">Search query that led to finding the GIF</param>
    /// <returns></returns>
    Task RegisterShareAsync(string id, string? query = null);

}
