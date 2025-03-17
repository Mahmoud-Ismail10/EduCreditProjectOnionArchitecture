using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<(TokenResponseDto?, ApiResponse)> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<TokenResponseDto>> RefreshTokenAsync(string refreshtoken);
        Task<ApiResponse> RegisterAsync(BaseUserDto person,Roles role, string RedirectUrl);
        Task<ApiResponse> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<ApiResponse> ConfirmEmailAsync(string userId, string token);
        Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string RedirectUrl);
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
