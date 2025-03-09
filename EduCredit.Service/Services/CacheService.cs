using Azure;
using EduCredit.Service.Services.Contract;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cache;

        public CacheService(IConnectionMultiplexer cache)
        {
            _cache = cache.GetDatabase();
        }
        public async Task<bool> DeleteCashAsync(string Key)
        {
            return await _cache.KeyDeleteAsync(Key);

        }

        public async Task<string?> GetCashAsync(string Key)
        {
            var response = await _cache.StringGetAsync(Key);
            if (response.IsNullOrEmpty) return null;
            return response;
        }

        public async Task SetAsync(string Key, object? Value, TimeSpan ExpirationDate)
        {
            if (Value is null)
                return ;
            var options= new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(Value, options);

             await _cache.StringSetAsync(Key, json);
        }
    }
}
