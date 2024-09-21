using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RBAC.DataDB;
using RBAC.Models;
using System.Security.Claims;

namespace RBAC.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Permission permission)
        {
            if (string.IsNullOrWhiteSpace(permission.Name))
            {
                return BadRequest("Please enter permission name");
            }
            var model = new PermissionsModel();
            var new_obj = await model.Add(permission);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await AuditLogModel.CreateLog($"Permission Created (Permission Id: {new_obj.Id})", userId);
            return Ok(new_obj);
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = new PermissionsModel();
            var permissions = await model.GetPermissions();
            return Ok(permissions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = new PermissionsModel();
            var _obj = await model.GetById(id);
            return _obj != null ? Ok(_obj) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Permission permission)
        {
            if (string.IsNullOrWhiteSpace(permission.Name))
            {
                return BadRequest("Please enter permission name");
            }
            var model = new PermissionsModel();
            var _obj = await model.GetById(id);
            if (_obj != null)
            {
                var new_obj = await model.Update(permission);
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await AuditLogModel.CreateLog($"Permission Updated (Permission Id: {new_obj.Id})", userId);
                return Ok(new_obj);
            }
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = new PermissionsModel();
            var _obj = await model.GetById(id);
            if (_obj != null)
            {
                await model.Delete(id);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await AuditLogModel.CreateLog($"Permission Deleted (Permission Id: {id})", userId);
                return NoContent();
            }
            return NotFound();
        }
        [HttpGet("{id}/AssignToRole/{role_id}")]
        public async Task<IActionResult> AssignToRole(int id, int role_id)
        {
            var permissionsModel = new PermissionsModel();
            var permission = await permissionsModel.GetById(id);
            if (permission == null)
            {
                return BadRequest("Invalid Permission");
            }
            var rolesModel = new RolesModel();
            var role = await rolesModel.GetById(id);
            if(role == null)
            {
                return BadRequest("Invalid Role");
            }
            var role_permission = await permissionsModel.GetRolePermission(role_id, id);
            if(role_permission != null)
            {
                return BadRequest("Permission is already assigned Role");
            }
            role_permission = new RolePermission() { PermissionId = id, RoleId = role_id };
            await permissionsModel.AddRolePermission(role_permission);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await AuditLogModel.CreateLog($"Permission Id: {id} Assigned To Role Id: {role_id}", userId);
            return Ok();
        }
        [HttpGet("{id}/UnassignFromRole/{role_id}")]
        public async Task<IActionResult> UnassignFromRole(int id, int role_id)
        {
            var permissionsModel = new PermissionsModel();
            var permission = await permissionsModel.GetById(id);
            if (permission == null)
            {
                return BadRequest("Invalid Permission");
            }
            var rolesModel = new RolesModel();
            var role = await rolesModel.GetById(id);
            if (role == null)
            {
                return BadRequest("Invalid Role");
            }
            var role_permission = await permissionsModel.GetRolePermission(role_id, id);
            if (role_permission == null)
            {
                return BadRequest("Permission is already not assigned Role");
            }
            await permissionsModel.DeleteRolePermission(role_id, id);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await AuditLogModel.CreateLog($"Permission Id: {id} Un assigned From Role Id: {role_id}", userId);
            return Ok();
        }
    }
}
