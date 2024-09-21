using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApi.DataDB;
using ProjectManagementApi.Models;
using System.Security.Claims;

namespace ProjectManagementApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectTask _obj)
        {
            if (!_obj.AreDatesValid())
            {
                return BadRequest("Start Date Cannot be Greater Than End Date");
            }
            _obj.ValidatePriority();
            _obj.ValidateStatus();

            var projectTasksModel = new ProjectTasksModel();
            var new_obj = await projectTasksModel.Add(_obj);
            return Ok(new_obj);
        }
        [HttpGet("List/{project_id}")]
        public async Task<IActionResult> List(int project_id)
        {
            var projectTasksModel = new ProjectTasksModel();
            var tasks = await projectTasksModel.GetProjectTasks(project_id);
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var projectTasksModel = new ProjectTasksModel();
            var _obj = await projectTasksModel.GetById(id);
            if (_obj != null)
            {
                return Ok(_obj);

            }
            return NotFound();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectTask task)
        {
            if (!task.AreDatesValid())
            {
                return BadRequest("Start Date Cannot be Greater Than End Date");
            }
            task.ValidatePriority();
            task.ValidateStatus();
            var projectTasksModel = new ProjectTasksModel();
            var _obj = await projectTasksModel.GetById(task.TaskId);
            if (_obj != null)
            {
                if (task.Status != _obj.Status)
                {
                    if (_obj.IsValidStatusTransition(task.Status))
                    {
                        return BadRequest("Status Transition is Invalid");
                    }

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await projectTasksModel.AddTaskLog(new TaskLog() { TaskId = task.TaskId, old_status = _obj.Status, new_status = task.Status, created_by = int.Parse(userId), created_at = DateTime.Now });
                }

                var new_obj = await projectTasksModel.Update(task);
                //send notifications
                var projectsModel = new ProjectsModel();
                var project = await projectsModel.GetById(new_obj.ProjectId);

                var userModel = new UserModel();
                var user = await userModel.GetById(new_obj.TaskOwnerId);
                await projectTasksModel.NotifyTaskMembers(id, "Task Updated", $@"
                    Project Title: {project.ProjectTitle} <br>
                    Project Description: {project.ProjectDescription} <br>
                    Task Title: {new_obj.TaskTitle} <br>
                    Start Date: {new_obj.StartDate} <br>
                    End Date: {new_obj.EndDate} <br>
                    Priority: {new_obj.Priority} <br>
                    Status: {new_obj.Status} <br>
                    Task Owner: {user.UserName} <br>
                ");
                return Ok(new_obj);
            }
            return NotFound();
        }

        [HttpGet("RecurrencePatterns")]
        public async Task<IActionResult> RecurrencePatterns()
        {
            var projectTasksModel = new ProjectTasksModel();
            var patterns = await projectTasksModel.GetRecurrencePatterns();
            return Ok(patterns);
        }
        [HttpGet("{id}/Members")]
        public async Task<IActionResult> TaskMembers(int id)
        {
            var projectTasksModel = new ProjectTasksModel();
            var task = await projectTasksModel.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            var members = await projectTasksModel.GetTaskMembers(id);
            return Ok(members);
        }
        [HttpPost("{id}/AssignToMember/{user_id}")]
        public async Task<IActionResult> AssignToMember(int id, int user_id)
        {
            var projectTasksModel = new ProjectTasksModel();
            var task = await projectTasksModel.GetById(id);
            if (task == null)
            {
                return NotFound("Invalid Task");
            }
            var userModel = new UserModel();
            var user = await userModel.GetById(user_id);
            if (user == null)
            {
                return NotFound("Invalid User");
            }
            var result = await projectTasksModel.AssignTaskToMember(id, user_id);
            if (result)
            {
                var projectsModel = new ProjectsModel();
                var project = await projectsModel.GetById(task.ProjectId);
                var taskOwner = await userModel.GetById(task.TaskOwnerId);
                await projectTasksModel.NotifyTaskMembers(id, "Task Assigned To New Member", $@"
                    Project Title: {project.ProjectTitle} <br>
                    Project Description: {project.ProjectDescription} <br>
                    Task Title: {task.TaskTitle} <br>
                    Start Date: {task.StartDate} <br>
                    End Date: {task.EndDate} <br>
                    Priority: {task.Priority} <br>
                    Status: {task.Status} <br>
                    Task Owner: {taskOwner.UserName} <br>
                    Assigned Member: {user.UserName} <br>
                ");
                return Ok();
            }

            return BadRequest("Task is already assigned to this Member");
        }
    }
}
