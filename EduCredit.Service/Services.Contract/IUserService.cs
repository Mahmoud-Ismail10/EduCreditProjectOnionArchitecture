using EduCredit.Service.DTOs.UserDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services.Contract
{
    public interface IUserService
    {
        Task<ApiResponse<GetUserInfoDto>> GetUserInfoAsync(string? userId, string? userRole);
    }
}
