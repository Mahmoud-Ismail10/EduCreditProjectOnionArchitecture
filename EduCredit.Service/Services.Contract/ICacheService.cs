using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface ICacheService
    {
        Task SetAsync(string Key, object? Value, TimeSpan ExpirationDate);
        Task<string?> GetCashAsync(string CacheKey);
        Task<bool> DeleteCashAsync(string Key);
    }
}
