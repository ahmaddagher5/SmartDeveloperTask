using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.DataDB;
using ProjectManagementApi.Helpers;

namespace ProjectManagementApi.Models
{
    public class ProjectsModel
    {
        private void ApplyProjectsQuery(ref IQueryable<Project> query, string? filter = null, string sort = "")
        {
            

            // Apply filtering
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.ProjectTitle.Contains(filter) || p.ProjectDescription.Contains(filter));
            }

            // Apply sorting
            query = sort switch
            {
                "ProjectTitle" => query.OrderBy(p => p.ProjectTitle),
                "StartDate" => query.OrderBy(p => p.StartDate),
                "EndDate" => query.OrderBy(p => p.EndDate),
                _ => query.OrderBy(p => p.ProjectId) // Default to sorting by id
            };

        }
        public async Task<List<Project>> GetProjects(string? filter = null, string sort = "", int page = 1, int pageSize = 10)
        {
            using (var context = new AppDbContext())
            {
                var query = context.Projects.AsQueryable();
                ApplyProjectsQuery(ref query, filter, sort);
                var projects = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                return projects;
            }
        }
        public async Task<int> GetProjectsCount(string? filter = null)
        {
            using (var context = new AppDbContext())
            {
                var query = context.Projects.AsQueryable();
                ApplyProjectsQuery(ref query, filter);
                return await query.CountAsync();
            }
        }
        public async Task<Project> Add(Project _obj)
        {
            using (var context = new AppDbContext())
            {
                context.Projects.Add(_obj);
                await context.SaveChangesAsync();
                return _obj;
            }
        }
        public async Task<Project?> GetById(int id)
        {
            using (var context = new AppDbContext())
            {
                var _obj = await context.Projects.SingleOrDefaultAsync(x => x.ProjectId == id);
                return _obj;
            }
        }
        public async Task<Project> Update(Project _obj)
        {
            using (var context = new AppDbContext())
            {
                var row = await context.Projects.SingleOrDefaultAsync(x => x.ProjectId == _obj.ProjectId);
                if (row != null)
                {
                    row.ProjectTitle = _obj.ProjectTitle;
                    row.ProjectDescription = _obj.ProjectDescription;
                    row.StartDate = _obj.StartDate;
                    row.EndDate = _obj.EndDate;
                    row.Status = _obj.Status;
                    row.Deleted = _obj.Deleted;
                    await context.SaveChangesAsync();
                }
                return _obj;
            }
        }
    }
}
