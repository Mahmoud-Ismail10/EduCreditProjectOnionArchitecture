using EduCredit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Services.Contract
{
    public interface ITokenService
    {
        string GenerateAccessToken(string email, string role, string UserId);
        RefreshToken GenerateRefreshToken();
    }
}
