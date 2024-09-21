using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.DataDB;
using ProjectManagementApi.Helpers;

namespace ProjectManagementApi.Models
{
    public class ProjectTasksModel
    {
        public async Task<ProjectTask> Add(ProjectTask _obj)
        {
            using (var context = new AppDbContext())
            {
                context.Tasks.Add(_obj);
                await context.SaveChangesAsync();
                return _obj;
            }
        }
        public async Task<List<ProjectTask>> GetProjectTasks(int project_id)
        {
            using (var context = new AppDbContext())
            {
                var tasks = await context.Tasks.Where(x=>x.ProjectId == project_id).ToListAsync();
                return tasks;
            }
        }
        public async Task<ProjectTask?> GetById(int id)
        {
            using (var context = new AppDbContext())
            {
                var _obj = await context.Tasks.SingleOrDefaultAsync(x => x.TaskId == id);
                return _obj;
            }
        }
        public async Task<ProjectTask> Update(ProjectTask _obj)
        {
            using (var context = new AppDbContext())
            {
                var row = await context.Tasks.SingleOrDefaultAsync(x => x.TaskId == _obj.TaskId);
                if (row != null)
                {
                    row.TaskTitle = _obj.TaskTitle;
                    row.StartDate = _obj.StartDate;
                    row.EndDate = _obj.EndDate;
                    row.Priority = _obj.Priority;
                    row.Status = _obj.Status;
                    row.TaskOwnerId = _obj.TaskOwnerId;
                    await context.SaveChangesAsync();
                }
                return _obj;
            }
        }
        public async Task<TaskLog> AddTaskLog(TaskLog _obj)
        {
            using (var context = new AppDbContext())
            {
                context.TaskLogs.Add(_obj);
                await context.SaveChangesAsync();
                return _obj;
            }
        }
        public async Task<List<RecurrencePattern>> GetRecurrencePatterns()
        {
            using (var context = new AppDbContext())
            {
                var _obj = await context.RecurrencePatterns.ToListAsync();
                return _obj;
            }
        }
        public async Task<List<User>> GetTaskMembers(int task_id)
        {
            using (var context = new AppDbContext())
            {
                var task_members = await context.TaskMembers
                                    .Join(context.Tasks, tm => tm.TaskId, t => t.TaskId, (tm, t) => new { tm.UserId })
                                    .Join(context.Users, tm => tm.UserId, u => u.UserId, (tm, u) => new { u })
                                    .Select(x => x.u)
                                    .ToListAsync();
                return task_members;
            }

        }
        public async Task<bool> AssignTaskToMember(int task_id, int user_id)
        {
            using (var context = new AppDbContext())
            {
                var task_member_count = await context.TaskMembers.Where(x=>x.TaskId == task_id && x.UserId==user_id).CountAsync();
                if (task_member_count == 0)
                {
                    var task_member = new TaskMember() { UserId = user_id, TaskId = task_id };
                    context.TaskMembers.Add(task_member);
                    await context.SaveChangesAsync();
                }
                return task_member_count == 0;
            }
        }
        public async Task NotifyTaskMembers(int task_id, string subject, string message)
        {
            var members = await GetTaskMembers(task_id);
            var members_emails = members.Select(x=>x.UserEmail).ToList();
            foreach (var email in members_emails)
            {
                if (MyHelper.IsValidEmail(email))
                {
                    MyHelper.SendMail(email, subject, message);
                }
            }
        }
    }
}
