using AutoMapper;
using EduCredit.APIs.DTOs.AuthDTOs;
using EduCredit.APIs.Errors;
using EduCredit.Core.Models;
using EduCredit.Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduCredit.APIs.Controllers
{
   
    public class AccountController : BaseApiController
    {

        private readonly UserManager<Person> _userManager;
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Person> userManager, IAuthService auth,IMapper mapper)
        {
            _userManager = userManager;
            _auth = auth;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var response = await _auth.LoginAsync(loginDto.Email,loginDto.Password);
            if (response == null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password."));
            }
            return Ok(response);
        }
        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] string refreshToken)
        //{
        //    var response = await _auth.RefreshTokenAsync(refreshToken);

        //    if (response == null)
        //    {
        //        return Unauthorized(new ApiResponse(401, "Invalid or expired refresh token."));
        //    }

        //    return Ok(response);
        //}
    }
}
