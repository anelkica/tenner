using System.Threading.Tasks;
using Tenner.Core.Models;

namespace Tenner.Avalonia.Services;

public interface ITennerClient
{
    Task<TenorResponse?> SearchAsync(string query, int limit = 20, string? pos = null);
    Task<TenorResponse?> GetFeaturedAsync(int limit = 20, string? pos = null);
    Task<TenorSuggestionsResponse?> GetAutocompleteAsync(string query, int limit = 3);
    Task RegisterShareAsync(string id, string? query = null);
}
