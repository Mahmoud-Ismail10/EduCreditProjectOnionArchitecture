﻿using EduCredit.Service.Services.Contract;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace EduCredit.Service.Services
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IDatabase _db;
        public TokenBlacklistService(IConnectionMultiplexer Db) 
        {
              _db = Db.GetDatabase();
        }
        public async Task AddTokenToBlacklistAsync(string token,TimeSpan expiry)
        {
            try
            {
                await _db.StringSetAsync($"blacklist:{token}", "invalid", expiry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error: {ex.Message}");
                return ;
            }
        }
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            try
            {
                return await _db.KeyExistsAsync($"blacklist:{token}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error: {ex.Message}");
                return false;
            }
        }

    }
}
