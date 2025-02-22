
using EduCredit.Core;
using EduCredit.Core.Models;
using EduCredit.Core.Services.Contract;
//using EduCredit.Core.Specifications.RefreshTokenSpecifications;
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
        private readonly IUnitOfWork _unitofWork;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<Person> userManager, ITokenService tokenService, ILogger<AuthService> logger, IUnitOfWork unitofWork)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitofWork = unitofWork;
            _logger = logger;
        }


        public async Task<string?> LoginAsync(string email, string password, Enum Role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "Invalid Email !";
            }
            //check if the password is correct
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                return "Invalid password !";
            }
            //check if Role is correct
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (!(role == Role.ToString()) || role == null)
            {
                return "UnAuthorized !";
            }

            return null;
        }


        #region Old Version
        //private async Task<string> GenerateAndStoreTokensAsync(Person user, RefreshToken? oldRefreshToken = null)
        //{

        //}
        //public async Task<string?> RefreshTokenAsync(string refreshToken)
        //{
        //    var spec = new RefreshTokenByTokenSpecifications(refreshToken);
        //    var existingRefreshToken = await _unitofWork.Repository<RefreshToken>()
        //        .GetByIdSpecificationAsync(spec);

        //    if (existingRefreshToken == null || existingRefreshToken.ExpiryDate < DateTime.UtcNow || existingRefreshToken.IsRevoked)
        //    {
        //        return "Invalid or expired refresh token attempt.";
        //    }

        //    var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
        //    if (user == null)
        //    {
        //        return "User not found for refresh token.";
        //    }

        //    var tokens = await GenerateAndStoreTokensAsync(user, existingRefreshToken);
        //    return tokens;
        //}
        #endregion

        //Get RefreshToken
        //Check if it is expired
        //if it is expired generate new one
        //if it is not expired return it
        //public async Task<string> RefreshTokenAsync(string refreshToken)
        //{
        //    var spec = new RefreshTokenByTokenSpecifications(refreshToken);
        //    var existingRefreshToken = await _unitofWork.Repository<RefreshToken>()
        //        .GetByIdSpecificationAsync(spec);
        //    if (existingRefreshToken == null || existingRefreshToken.ExpiryDate < DateTime.UtcNow || existingRefreshToken.IsRevoked)
        //    {
        //        return "Invalid or expired refresh token attempt.";
        //    }
        //    var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
        //    if (user == null)
        //    {
        //        return "User not found for refresh token.";
        //    }
        //    var newRefreshToken = _tokenService.GenerateRefreshToken();
        //    existingRefreshToken.Token = newRefreshToken;
        //    await _unitofWork.Repository<RefreshToken>().UpdateAsync(existingRefreshToken);
        //    await _unitofWork.completeAsync();
        //    return newRefreshToken;
        //}
    }
}
