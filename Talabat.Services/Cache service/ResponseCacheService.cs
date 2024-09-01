using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Services.Cache_service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return;

            var serializedOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serializedResponse = JsonSerializer.Serialize(response , serializedOptions);

            await _database.StringSetAsync(cacheKey, serializedResponse ,timeToLive );
        }

        public async Task<string?> GetCacheResponseAsync(string cacheKey)
        {
            var response = await _database.StringGetAsync(cacheKey);

            if (response.IsNullOrEmpty) return null;

            return response;
        }
    }
}
