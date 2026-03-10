using System.Text.Json.Serialization;

namespace Tenner.API.Models;

public record TenorResponse
{
    [JsonPropertyName("results")]
    public GifResult[] Results { get; init; } = [];

    /// <summary>Pagination cursor for the next set of results.</summary>
    /// <remarks>
    /// Pass this value as the pos parameter in the next request to retrieve the following page.
    /// Returns an empty string when there are no more results.
    /// </remarks>
    /*
        sorry for the yap, i was working at night and just making sure i read it correctly lol
        Request 1: no pos → results 1-20  + next = "ABC..."
        Request 2: pos=ABC → results 21-40 + next = "DEF..."     
    */
    [JsonPropertyName("next")]
    public string Next { get; init; } = string.Empty;
}
