using System.Text.Json.Serialization;

namespace Tenner.Core.Models;

public record TenorSuggestionsResponse
{
    [JsonPropertyName("results")]
    public string[] Results { get; init; } = [];
}
