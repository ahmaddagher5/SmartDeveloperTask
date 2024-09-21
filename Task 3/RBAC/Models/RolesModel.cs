using Microsoft.EntityFrameworkCore;
using RBAC.DataDB;

namespace RBAC.Models
{
    public class RolesModel
    {
        public async Task<List<Role>> GetRoles()
        {
            using (var context = new AppDbContext())
            {
                var tasks = await context.Roles.ToListAsync();
                return tasks;
            }
        }
        public async Task<Role> Add(Role _obj)
        {
            using (var context = new AppDbContext())
            {
                context.Roles.Add(_obj);
                await context.SaveChangesAsync();
                return _obj;
            }
        }
        public async Task<Role?> GetById(int id)
        {
            using (var context = new AppDbContext())
            {
                var _obj = await context.Roles.SingleOrDefaultAsync(x => x.Id == id);
                return _obj;
            }
        }
        public async Task<Role> Update(Role _obj)
        {
            using (var context = new AppDbContext())
            {
                var row = await context.Roles.SingleOrDefaultAsync(x => x.Id == _obj.Id);
                if (row != null)
                {
                    row.Name = _obj.Name;
                    await context.SaveChangesAsync();
                }
                return _obj;
            }
        }
        public async Task<bool> Delete(int id)
        {
            using (var context = new AppDbContext())
            {
                var row = await context.Roles.FindAsync(id);
                if (row != null)
                {
                    context.Roles.Remove(row);
                    await context.SaveChangesAsync();
                }
                return row != null;
            }
        }
    }
}
