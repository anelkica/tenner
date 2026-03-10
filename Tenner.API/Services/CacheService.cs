using System.Text.Json;
using StackExchange.Redis;
using Tenner.API.Interfaces;

namespace Tenner.API.Services;

public class CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger) : ICacheService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            RedisValue value = await _db.StringGetAsync(key);
            if (!value.HasValue) return default;

            return JsonSerializer.Deserialize<T>((string) value!);
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Cache read failed for key: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        try
        {
            if (expiry.HasValue)
                await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expiry.Value);
            else
                await _db.StringSetAsync(key, JsonSerializer.Serialize(value));
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Cache set failed for key: {Key}", key);
        }
    }
}
