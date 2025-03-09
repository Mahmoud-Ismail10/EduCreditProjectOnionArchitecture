using AutoMapper;
using EduCredit.Core.Models;
using EduCredit.Service.DTOs.UserDTOs;
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
        private readonly IMapper _mapper;
        private readonly UserManager<Person> _userManager;

        public UserController(IMapper mapper, UserManager<Person> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("getuserbyemail")]
        [Cache(30)]
        [Authorize]
        [ProducesResponseType(typeof(GetUserInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetUserInfoDto>> GetUserByEmail(string userEmail)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
                return NotFound(new ApiResponse(404, "This User Is Not Found!"));

            if (string.IsNullOrWhiteSpace(userId) || userId != user.Id.ToString())
                return Unauthorized(new ApiResponse(401, "Unauthorized!"));

            var userDto = _mapper.Map<GetUserInfoDto>(user);
            return Ok(userDto);
        }
    
    }
}
