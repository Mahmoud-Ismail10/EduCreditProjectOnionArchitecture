using AutoMapper;
using EduCredit.Core.Models;
using EduCredit.Core.Security;
using EduCredit.Service.DTOs.AuthDTOs;
using EduCredit.Service.Errors;
using EduCredit.Service.Helper;
using EduCredit.Service.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCredit.APIs.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IMapper mapper,IUserService userService)
        {
           _userService = userService;
        }

        [HttpGet("GetUserInfo")]
        [Authorize]
        [ProducesResponseType(typeof(BaseRegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseRegisterDto>> GetUserInfo()
        {
            var userId = User.FindFirstValue("userId");
            var userRole = User.FindFirstValue("role");

            if (userId is null || userRole is null)
                return Unauthorized(new ApiResponse(401, "Unauthorized: Invalid user "));

            var result = await _userService.GetUserInfoAsync(userId, userRole);

            if (result == null)
                return NotFound(new ApiResponse(404, "User Not Found"));

            return Ok(result);
        }


    }
}
