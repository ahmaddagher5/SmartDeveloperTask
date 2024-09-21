using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApi.DataDB;
using ProjectManagementApi.Models;

namespace ProjectManagementApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Project _obj)
        {
            if (!_obj.AreDatesValid())
            {
                return BadRequest("Start Date Cannot be Greater Than End Date");
            }
            _obj.ValidateStatus();

            var projectsModel = new ProjectsModel();
            var new_obj = await projectsModel.Add(_obj);
            return Ok(new_obj);
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? filter = null, [FromQuery] string sort = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var projectsModel = new ProjectsModel();
            var projects = await projectsModel.GetProjects(filter, sort, page, pageSize);
            var projects_count = await projectsModel.GetProjectsCount(filter);
            return Ok(new
            {
                TotalItems = projects_count,
                Page = page,
                PageSize = pageSize,
                Data = projects
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var projectsModel = new ProjectsModel();
            var _obj = await projectsModel.GetById(id);
            if (_obj != null)
            {
                return Ok(_obj);

            }
            return NotFound();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Project project)
        {
            if (!project.AreDatesValid())
            {
                return BadRequest("Start Date Cannot be Greater Than End Date");
            }

            project.ValidateStatus();

            

            var projectsModel = new ProjectsModel();
            var _obj = await projectsModel.GetById(project.ProjectId);
            if (_obj != null)
            {
                if (_obj.IsValidStatusTransition(project.Status))
                {
                    return BadRequest("Status Transition is Invalid");
                }

                var new_obj = await projectsModel.Update(project);
                return Ok(new_obj);
            }
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var projectsModel = new ProjectsModel();
            var _obj = await projectsModel.GetById(id);
            if (_obj != null)
            {
                _obj.Deleted = true;
                var new_obj = await projectsModel.Update(_obj);
                return NoContent();
            }
            return NotFound();
        }
    }
}
