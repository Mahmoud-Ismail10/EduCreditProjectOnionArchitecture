using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Services.Contract
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string email, string role);
    }
}
