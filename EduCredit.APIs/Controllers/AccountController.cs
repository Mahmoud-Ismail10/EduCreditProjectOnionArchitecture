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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Person> userManager, IAuthService auth,IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _auth = auth;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        //[HttpPost("login")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status401Unauthorized)]
        //public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto loginDto)
        //{
        //    var response = await _auth.LoginAsync(loginDto.Email,loginDto.Password);
        //    if (response == null)
        //    {
        //        return Unauthorized(new ApiResponse(401, "Invalid email or password."));
        //    }
        //    return Ok(response);
        //}

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            #region Not Clean Code
            ////check if the model state is valid
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new ApiResponse(400, "Invalid model state."));
            //}
            ////check if the user exists
            //var user = await _userManager.FindByEmailAsync(loginDto.Email);
            //if (user == null)
            //{
            //    return Unauthorized(new ApiResponse(401, "This email is not exist !"));
            //}
            ////check if the password is correct
            //var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            //if (!result)
            //{
            //    return Unauthorized(new ApiResponse(401, "Invalid password."));
            //}
            ////check if Role is correct
            //var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            //if (!(role == loginDto.Role.ToString()) || role == null)
            //{
            //    return Unauthorized(new ApiResponse(401, "Un Authorized."));
            //}
            ////generate token
            //var token = _tokenService.GenerateAccessToken(user.Id, loginDto.Email, role);
            //return Ok(new TokenResponseDto
            //{
            //    AccessToken = token
            //});
            #endregion

            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Invalid Data !"));
            }
            var response = await _auth.LoginAsync(loginDto.Email, loginDto.Password, loginDto.Role);

            if (response != null)
            {
                return Unauthorized(new ApiResponse(401, response));
            }
            //generate token
            var token = _tokenService.GenerateAccessToken(loginDto.Email, loginDto.Role.ToString());

            return Ok(new TokenResponseDto
            {
                AccessToken = token
            });
         

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
