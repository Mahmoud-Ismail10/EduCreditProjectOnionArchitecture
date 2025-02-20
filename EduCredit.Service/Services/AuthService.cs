
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Person> _userManager;
        private readonly ITokenService _tokenService;
        //private readonly IUnitOfWork _unitofWork;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<Person> userManager, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
           // _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                _logger.LogWarning("Login failed: Email not found - {Email}", email);
                return $"Login failed: Email not found - {email}";
            }
            
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogWarning("Login failed: Incorrect password for {Email}", email);
                return $"Login failed: Incorrect password for {email}";
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";
            var token = _tokenService.GenerateAccessToken(user.Id, user.Email, role);

            _logger.LogInformation("User {Email} successfully logged in.", email);
            return token;
        }



        //private async Task<object> GenerateAndStoreTokensAsync(Person user, RefreshToken? oldRefreshToken = null)
        //{
        //    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Student";
        //    var requestToken = BuildTokenRequest(user, role);

        //    var tokens = _tokenService.GenerateToken(user.Id,user.Email,role);

        //    if (oldRefreshToken != null)
        //    {
        //        oldRefreshToken.IsRevoked = true;
        //       await _unitofWork.Repository<RefreshToken>().UpdateAsync(oldRefreshToken);
        //    }

        //    var newRefreshToken = new RefreshToken
        //    {
        //        Token = tokens.ToString()!,
        //        ExpiryDate = DateTime.UtcNow.AddDays(7),
        //        UserId = user.Id
        //    };

        //    await _unitofWork.Repository<RefreshToken>().CreateAsync(newRefreshToken);
        //    await _unitofWork.completeAsync();

        //    return tokens;
        //}

        //private object BuildTokenRequest(Person user, string role)
        //{
        //    return new 
        //    {
        //        email = user.Email,
        //        ExpireDate = DateTime.UtcNow,
        //        Role = role,
        //        UserId = user.Id
        //    };
        //}

        //public async Task<object> RefreshTokenAsync(string refreshToken)
        //{
        //    var spec = new RefreshTokenByTokenSpecifications(refreshToken);
        //    var existingRefreshToken = await _unitofWork.Repository<RefreshToken>()
        //        .GetByIdSpecificationAsync(spec);

        //    if (existingRefreshToken == null || existingRefreshToken.ExpiryDate < DateTime.UtcNow || existingRefreshToken.IsRevoked)
        //    {
        //        _logger.LogWarning("Invalid or expired refresh token attempt.");
        //        return null;
        //    }

        //    var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
        //    if (user == null)
        //    {
        //        _logger.LogWarning("User not found for refresh token.");
        //        return null;
        //    }

        //    var tokens = await GenerateAndStoreTokensAsync(user, existingRefreshToken);
        //    _logger.LogInformation("Refresh token successfully used for user {UserId}.", user.Id);
        //    return tokens;
        //}
    }
}
