using System.Text.Json.Serialization;

namespace Tenner.Core.Models;

public record MediaFormat
{
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    [JsonPropertyName("duration")]
    public double Duration { get; init; }

    /// <summary>Dimensions of the media [0] = width, [1] = height.</summary>
    [JsonPropertyName("dims")]
    public int[] Dims { get; init; } = [];

    /// <summary>File size in bytes.</summary>
    [JsonPropertyName("size")]
    public long Size { get; init; }
}
