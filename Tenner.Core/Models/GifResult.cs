using System.Text.Json.Serialization;

namespace Tenner.Core.Models;

public record GifResult
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>SEO Auto-generated title of the post.</summary>
    [JsonPropertyName("h1_title")] // the real title property is always empty, for some reason?
    public string Title { get; init; } = string.Empty;

    /// <summary>Short URL pointing to the post on Tenor, typically for sharing.</summary>
    /// <remarks>Example: https://tenor.com/bItJt.gif</remarks>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    /// <summary>Long URL pointing to the post on Tenor.</summary>
    /// <remarks>Example: https://tenor.com/view/rickroll-roll-rick-never-gonna-give-you-up-never-gonna-gif-22954713</remarks>
    [JsonPropertyName("itemurl")]
    public string ItemUrl { get; init; } = string.Empty;

    /// <summary>SEO Auto-generated description of the post.</summary>
    [JsonPropertyName("content_description")]
    public string ContentDescription { get; init; } = string.Empty;

    [JsonPropertyName("media_formats")]
    public Dictionary<string, MediaFormat> MediaFormats { get; init; } = [];

    /// <summary>Array of string tags associated with the post.</summary>
    /// <remarks>Example: ["cats", "animals", "cute"]</remarks>
    [JsonPropertyName("tags")]
    public string[] Tags { get; init; } = [];

    /// <summary>Unix epoch timestamp of creation.</summary>
    [JsonPropertyName("created")]
    public double Created { get; init; }

    /// <summary>Number of times the post was shared.</summary>
    [JsonPropertyName("shares")]
    public int Shares { get; init; }
}
