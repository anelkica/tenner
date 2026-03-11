using System.Text.Json.Serialization;
namespace Tenner.API.Models;

public record TenorSuggestionsResponse
{
    [JsonPropertyName("results")]
    public string[] Results { get; init; } = [];
}
