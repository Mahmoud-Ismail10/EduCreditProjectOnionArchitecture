using AutoMapper;
using Azure;
using EduCredit.Core.Enums;
using EduCredit.Core.Models;
using EduCredit.Core.Security;
using EduCredit.Core.Services.Contract;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace EduCredit.APIs.Controllers
{
   
    public class AccountController : BaseApiController
    {
        #region Fields
        private readonly IAuthService _auth;
        private readonly ITokenService _tokenService;
        private readonly ITokenBlacklistService _blacklistService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        #endregion

        #region Constructor
        public AccountController( IAuthService auth, ITokenService tokenService,ITokenBlacklistService blacklistService,IMapper mapper,ICacheService cacheService)
        {
            _auth = auth;
            _tokenService = tokenService;
            _blacklistService = blacklistService;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        #endregion

        #region Endpoints
        #region Register
        //[HttpPost("AdminRegistration")]
        //[Authorize(Roles= AuthorizationConstants.SuperAdminRole)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult> Register([FromBody] RegisterAdminDto registerDto, Roles role, string RedirectUrl)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new ApiResponse(400, "Invalid Data!"));
        //    // Mapping RegisterAdminDto to Person
        //    var Mappedadmin = _mapper.Map<RegisterAdminDto, Person>(registerDto);
        //    var result = await _auth.RegisterAsync(Mappedadmin,role,RedirectUrl);
        //    if (result.StatusCode != 200)
        //        return BadRequest(new ApiResponse(400, result.Message));

        //    return Created();
        //}

        [HttpPost("UserRegistration")]
        [Authorize(Roles = $"{AuthorizationConstants.SuperAdminRole},{AuthorizationConstants.AdminRole}")]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] BaseUserDto registerDto,Roles role, string RedirectUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Data!"));
            var result = await _auth.RegisterAsync(registerDto, role, RedirectUrl);
            if (result.StatusCode != 200)
                return BadRequest(new ApiResponse(400, result.Message));
            return Ok(new ApiResponse(201,"Success"));
        }
        #endregion

        #region ConfirmEmail
        [HttpGet("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token, [FromQuery] string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(redirectUrl))
                return BadRequest(new ApiResponse(400, "Invalid data!"));

            var result = await _auth.ConfirmEmailAsync(userId, token);
            // If the confirmation fails, return a bad request
            if (result.StatusCode!=200)
            {
                return BadRequest(new ApiResponse(400,result.Message));
            }
            // Redirect the user to the frontend after successful confirmation
            return Redirect($"{redirectUrl}?status=success");
        }

        #endregion

        #region Login
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    
        public async Task<ActionResult<ApiResponse<TokenResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Data!"));

            var (tokens, errors) = await _auth.LoginAsync(loginDto);

            if (errors.StatusCode != 200)
                return Unauthorized(new ApiResponse(401, errors.Message));

            return new ApiResponse<TokenResponseDto>(200, "Success",tokens);
        }
        #endregion

        #region RefreshToken

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<TokenResponseDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<TokenResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var response = await _auth.RefreshTokenAsync(request.RefreshToken);

            if (response.StatusCode!=200)
                return Unauthorized(new ApiResponse(401, response.Message));

            return Ok(new ApiResponse<TokenResponseDto>(200,"Success",response.Result));
        }
        #endregion

        #region Logout
        [HttpGet("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ","");
            if (string.IsNullOrWhiteSpace(token))
                return Unauthorized(new ApiResponse(401, "UnAuthrize!"));
            var expiry = TimeSpan.FromHours(1);
            await _blacklistService.AddTokenToBlacklistAsync(token, expiry);
            await _cacheService.DeleteCashAsync(token);
            return Ok(new ApiResponse (200, "Logged out successfully"));
        }
        #endregion

        #region ForgotPassword
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto, string RedirectUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Data!"));
            var response = await _auth.ForgotPasswordAsync(forgotPasswordDto, RedirectUrl);
            if (response.StatusCode != 200)
                return BadRequest(new ApiResponse(400, response.Message));
            return Ok(new ApiResponse(200, "Password reset link sent successfully"));
        }
        #endregion

        #region ResetPassword
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Data!"));
            var response = await _auth.ResetPasswordAsync(resetPasswordDto);
            if (response.StatusCode != 200)
                return BadRequest(new ApiResponse(400, response.Message));
            return Ok(new ApiResponse(200, "Password reset successfully"));
        }
        #endregion

        #region changePassword
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Data!"));

            ///  UserId From Token
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            if (userId != changePasswordDto.user_Id)
                return Unauthorized(new ApiResponse(401, "User not authenticated"));
            var response = await _auth.ChangePasswordAsync(userId, changePasswordDto);

            if (response.StatusCode != 200)
                return BadRequest(new ApiResponse(400, response.Message));

            return Ok(new ApiResponse(200, "Password changed successfully"));
        }


        #endregion

        #endregion
    }
}
