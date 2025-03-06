using EduCredit.Service.Services.Contract;
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
            await _db.StringSetAsync($"blacklist:{token}", "invalid", expiry);
        }
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _db.KeyExistsAsync($"blacklisted:{token}");
        }
    }
}
