using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IUserService
    {
        Task<BaseRegisterDto?> GetUserInfoAsync(string? userId, string? userRole);
        Guid GetUserGuidFromClaims(ClaimsPrincipal user);
    }
}
