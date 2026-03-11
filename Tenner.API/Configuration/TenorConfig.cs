namespace Tenner.API.Configuration;

public record TenorConfig
{
    public string ApiKey { get; init; } = string.Empty;
    public string ClientKey { get; init; } = string.Empty;
    public string MediaFilter { get; init; } = "gif,tinygif,tinymp4";
}
