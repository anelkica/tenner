using Tenner.API.Models;

namespace Tenner.API.Interfaces;

public interface ITenorService
{
    Task<TenorResponse?> SearchAsync(string query, int limit = 10, string? pos = null);
}
