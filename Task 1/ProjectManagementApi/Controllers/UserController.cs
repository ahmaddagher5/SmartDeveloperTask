using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementApi.DataDB;
using ProjectManagementApi.Helpers;
using ProjectManagementApi.Models;
using ProjectManagementApi.Objects;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public UserController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginObj objUser)
        {
            if (string.IsNullOrWhiteSpace(objUser.UserPhone) || string.IsNullOrWhiteSpace(objUser.Password))
            {
                return BadRequest("Please fill all blanks");
            }
            var userModel = new UserModel();
            var user = await userModel.Login(objUser.UserPhone, MyHelper.SHA1Encode(objUser.Password));
            if (user != null)
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized("Invalid Userphone or Password");
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterObj obj)
        {
            if (string.IsNullOrWhiteSpace(obj.UserName) || string.IsNullOrWhiteSpace(obj.Password)|| string.IsNullOrWhiteSpace(obj.ConfirmPassword) || string.IsNullOrWhiteSpace(obj.UserEmail) || string.IsNullOrWhiteSpace(obj.UserPhone))
            {
                return BadRequest("Please fill all blanks"); 
            }
            if (!MyHelper.IsValidEmail(obj.UserEmail))
            {
                return BadRequest("User Email is invalid");
            }
            if (!obj.Password.Equals(obj.ConfirmPassword))
            {
                return BadRequest("Passwords are not matched");
            }
            var userModel = new UserModel();
            if (await userModel.IsUserPhoneExisting(obj.UserPhone))
            {
                return BadRequest("User Phone is already registered");
            }
            var objUser = new User(obj);
            var user = await userModel.Register(objUser);
            return Ok(user);
        }
    }
}
