using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAC.DataDB;
using RBAC.Models;
using System.Security.Claims;

namespace RBAC.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                return BadRequest("Please enter role name");
            }
            var model = new RolesModel();
            var new_obj = await model.Add(role);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await AuditLogModel.CreateLog($"Role Created (Role Id: {new_obj.Id})", userId);

            return Ok(new_obj);
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = new RolesModel();
            var roles = await model.GetRoles();
            return Ok(roles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = new RolesModel();
            var _obj = await model.GetById(id);
            return _obj != null ? Ok(_obj) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                return BadRequest("Please enter role name");
            }
            var model = new RolesModel();
            var _obj = await model.GetById(id);
            if (_obj != null)
            {
                var new_obj = await model.Update(role);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await AuditLogModel.CreateLog($"Role Updated (Role Id: {new_obj.Id})", userId);
                return Ok(new_obj);
            }
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = new RolesModel();
            var _obj = await model.GetById(id);
            if (_obj != null)
            {
                await model.Delete(id);

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await AuditLogModel.CreateLog($"Role Deleted (Role Id: {id})", userId);
                return NoContent();
            }
            return NotFound();
        }
    }
}
