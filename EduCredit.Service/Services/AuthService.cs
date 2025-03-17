using EduCredit.Core;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Services.Contract;
using EduCredit.Core.Specifications.RefreshTokenSpecifications;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Parameters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Web;

namespace EduCredit.Service.Services
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly UserManager<Person> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitofWork;
        private readonly IEmailServices _emailService;
        #endregion

        #region Constructor
        public AuthService(UserManager<Person> userManager, ITokenService tokenService, IUnitOfWork unitofWork,IEmailServices emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitofWork = unitofWork;
            _emailService = emailService;
        }
        #endregion

        #region Methods
        #region Register

        public async Task<ApiResponse> RegisterAsync(BaseUserDto person, Roles role, string RedirectUrl)
        {
            if (await _userManager.FindByEmailAsync(person.Email) is not null)
                return new ApiResponse(400, "This email is already registered!");

            var user = CreateUser(person, role);
            if (user is null)
                return new ApiResponse(400, "Invalid role!");

            var result = await _userManager.CreateAsync(user, user.NationalId);
            if (!result.Succeeded)
                return new ApiResponse(400, $"Failed to register! {string.Join(", ", result.Errors.Select(e => e.Description))}");

            var roleName = role.ToString();
            if (!(await _userManager.AddToRoleAsync(user, roleName)).Succeeded)
                return new ApiResponse(400, "Failed to assign the role!");

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = GenerateConfirmationUrl(user.Id, emailToken, RedirectUrl);

            var emailSent = await _emailService.SendEmailAsync(person.Email, confirmationUrl, EmailType.ConfirmEmail);
            if (emailSent.StatusCode != 200) 
                return new ApiResponse(400, "Failed to send confirmation email!");
            await _unitofWork.CompleteAsync();
            return new ApiResponse(200, "User registered successfully! Please confirm your email.");
        }

        private string GenerateConfirmationUrl(Guid userId, string token, string redirectUrl)
        {
            var EncodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            var confirmationUrl = $"https://educredit.runasp.net/api/Account/confirm-email?userId={userId}&token={EncodedToken}&redirectUrl={redirectUrl}";
            return confirmationUrl; 
        }

        private Person? CreateUser(BaseUserDto person, Roles role)
        {
            string userName = person.FullName.Replace(" ", "");

            Person? user = role switch
            {
                Roles.StudentRole => new Student
                {
                    GPA = 0.0f,
                    Level = Level.First,
                    DepartmentId = person.DepartmentId == Guid.Empty ? null : person.DepartmentId,
                    CreditHours = 0.0f,
                    TeacherId = null
                },
                Roles.TeacherRole => new Teacher
                {
                    AppointmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    DepartmentId = person.DepartmentId == Guid.Empty ? null : person.DepartmentId
                },
                Roles.AdminRole => new Admin(),
                _ => null
            };

            if (user is null)
                return null;

            user.Email = person.Email;
            user.UserName = userName;
            user.NationalId = person.NationalId;
            user.PhoneNumber = person.PhoneNumber;
            user.BirthDate = person.BirthDate;
            user.Gender = person.Gender;
            user.Address = person.Address;
            user.FullName = person.FullName;
            user.EmailConfirmed = false;

            return user;
        }

        #endregion

        #region ConfirmEmail
        public async Task<ApiResponse> ConfirmEmailAsync(string userId, string token)
        {
            // Find the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ApiResponse(404, "User not found!");

            // Confirm the email
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            Console.WriteLine(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            // Check if the email was confirmed successfully
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                Console.WriteLine(error.Description);
                return new ApiResponse(400, $"Failed to confirm the email! ");
            }

            return new ApiResponse(200, "Email confirmed successfully!");
        }

        #endregion

        #region Login
        public async Task<(TokenResponseDto?, ApiResponse)> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Check if the user exists
            if (user == null)
                return (null, new ApiResponse(401, "This email is not found!"));
            // Check if the password is correct
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return (null, new ApiResponse(401, "Invalid password!"));
            // Check if the role is correct
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.SingleOrDefault();

            //if (role is null || role != loginDto.Role.ToString())
            //    return (null, new ApiResponse(400, "This email is not found!"));
            if (!user.EmailConfirmed)
                return (null,new ApiResponse(400, "You need to confirm your email before logging in!"));
            // Generate the access token
            var accessToken = _tokenService.GenerateAccessToken(user.Email, role, user.Id.ToString());
            // Check if the user has a refresh token
            var refreshToken = user.RefreshTokens.FirstOrDefault(t => !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow);

            if (refreshToken == null)
            {
                refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await _unitofWork.CompleteAsync();
            }
            // Return the access token and refresh token
            return (new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            }, new ApiResponse(200, "Login Successful!"));
        }
        #endregion

        #region RefreshToken
        public async Task<ApiResponse<TokenResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            // Check if the refresh token is empty
            if (string.IsNullOrWhiteSpace(refreshToken))
                return new ApiResponse<TokenResponseDto>(400, "Refresh token is required.");

            // Check if the refresh token exists
            var spec = new RefreshTokenByTokenSpecifications(refreshToken);
            var existingRefreshToken = await _unitofWork.Repository<RefreshToken>().GetByIdSpecificationAsync(spec);

            if (existingRefreshToken is null || existingRefreshToken.ExpiryDate < DateTime.UtcNow || existingRefreshToken.IsRevoked )
            {
                return new ApiResponse<TokenResponseDto>(400, "Invalid or expired refresh token, please log in again!");
            }
            // Get the user with the refresh token
            var user = await _userManager.Users
           .Include(u => u.RefreshTokens)
           .FirstOrDefaultAsync(u => u.RefreshTokens.FirstOrDefault() == existingRefreshToken);
            existingRefreshToken.IsRevoked = true;
            //
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            // Generate the new access token
            var accessToken = _tokenService.GenerateAccessToken(existingRefreshToken.Person.Email, role,user.Id.ToString());

            //Generate a new refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            existingRefreshToken.Person.RefreshTokens.Add(newRefreshToken);

            // save the changes
            await _unitofWork.CompleteAsync();
            // Return the new access token and refresh token
            return new ApiResponse<TokenResponseDto>(200, "Refresh Token Generated Successfully!", new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            });
        }


        #endregion

        #region ForgotPassword
        public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto,string RedirectUrl)
        {
            // Find the user
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return new ApiResponse(404, "User not found!");
            // Generate and encode password reset token
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(passwordResetToken);
            Console.WriteLine($"encodedToken: {encodedToken}");
            var passwordResetUrl = $"{RedirectUrl}?userId={user.Id}&token={encodedToken}";

            // Send password reset email
            var emailSent = await _emailService.SendEmailAsync(forgotPasswordDto.Email, passwordResetUrl, EmailType.ForgotPassword);
            if (emailSent.StatusCode != 200)
                return new ApiResponse(400, "Failed to send the email!");
            return new ApiResponse(200);
        }
        #endregion

        #region ResetPassword
        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            // Find the user
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return new ApiResponse(404, "User not found!");
            // Decode the token
            var decodedToken = WebUtility.UrlDecode(resetPasswordDto.Token).Trim(); // Securely decode token
            Console.WriteLine($"decodedToken: {decodedToken}");

            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);
            // Check if the password was reset successfully
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ApiResponse(400, $"Failed to reset the password! {errors} ");
            }
            return new ApiResponse(200, "Password reset successfully!");
        }
        #endregion

        #region ChangePassword

        public async Task<ApiResponse> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ApiResponse(404, "User not found");

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ApiResponse(400, $"Failed to change password: {errors}");
            }
            await _userManager.UpdateSecurityStampAsync(user);

            await _unitofWork.CompleteAsync();
            return new ApiResponse(200, "Password changed successfully!");
        }


        #endregion

        #endregion
    }
}
