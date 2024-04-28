using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;

public class RedisService
{
    private readonly IDatabase _redisDb;
    private readonly ConnectionMultiplexer _redis;
    private readonly IConfiguration _configuration;

    public RedisService(IConfiguration configuration)
    {
        _configuration = configuration;
        _redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        _redisDb = _redis.GetDatabase();
    }

    public async Task<bool> RegisterSessionAsync(string sessionId, Session session)
    {
        var sessionJson = JsonSerializer.Serialize(session);
        return await _redisDb.StringSetAsync(sessionId, sessionJson);   
    }

    public async Task<string?> GetSessionAsync(string sessionId)
    {
    var sessionJson = await _redisDb.StringGetAsync(sessionId);
    return sessionJson.HasValue ? sessionJson.ToString() : null;
    }
}