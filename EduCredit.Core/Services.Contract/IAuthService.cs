using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password,Enum role);
        //Task<object> RefreshTokenAsync(string refreshtoken);

    }
}
