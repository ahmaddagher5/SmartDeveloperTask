using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAC.DataDB;
using RBAC.Helpers;
using RBAC.Models;
using RBAC.Objects;
using System.Security.Claims;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public UsersController(ITokenService tokenService)
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
                var token = await _tokenService.GenerateToken(user);
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
            if (string.IsNullOrWhiteSpace(obj.UserName) || string.IsNullOrWhiteSpace(obj.Password) || string.IsNullOrWhiteSpace(obj.ConfirmPassword) || string.IsNullOrWhiteSpace(obj.UserEmail) || string.IsNullOrWhiteSpace(obj.UserPhone))
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
        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("{userId}/AssignRole/{roleId}")]
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            var userModel = new UserModel();
            var user = await userModel.GetById(userId);
            if (user == null)
            {
                return BadRequest("Invalid User");
            }
            var rolesModel = new RolesModel();
            var role = await rolesModel.GetById(roleId);
            if (role == null)
            {
                return BadRequest("Invalid Role");
            }
            user = await userModel.AssignRoleToUser(userId, roleId);
            var uId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await AuditLogModel.CreateLog($"Role Id: {roleId} Assigned To User Id: {uId}", userId);
            return Ok();
        }
    }
}
