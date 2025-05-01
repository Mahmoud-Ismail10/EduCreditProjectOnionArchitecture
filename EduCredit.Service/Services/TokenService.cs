using EduCredit.Core.Models;
using EduCredit.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Service.Services
{
    public class TokenService : ITokenService
    {
        #region Fields
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Methods

        #region GenerateAccessToken
        public string GenerateAccessToken(string email, string role, string UserId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                     //new Claim("email", email),
                     //new Claim("userId", UserId), 
                     //new Claim("role", role),
                     new Claim(ClaimTypes.Email, email),
                     new Claim(ClaimTypes.Role, role),
                     new Claim(ClaimTypes.NameIdentifier, UserId),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var expiryMinutes = double.Parse(jwtSettings["RememberMeAccessTokenExpiryMinutes"]);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region GenerateRefreshToken
        public RefreshToken GenerateRefreshToken()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expiryDays = int.Parse(jwtSettings["RefreshTokenExpiryDays"]);

            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiryDate = DateTime.UtcNow.AddDays(expiryDays)
            };
        }
        #endregion

        #endregion
    }

}
