using Microsoft.EntityFrameworkCore;
using RBAC.DataDB;

namespace RBAC.Models
{
    public class PermissionsModel
    {
        public async Task<List<Permission>> GetPermissions()
        {
            using (var context = new AppDbContext())
            {
                var tasks = await context.Permissions.ToListAsync();
                return tasks;
            }
        }
        public async Task<Permission> Add(Permission _obj)
        {
            using (var context = new AppDbContext())
            {
                context.Permissions.Add(_obj);
                await context.SaveChangesAsync();
                return _obj;
            }
        }
        public async Task<Permission?> GetById(int id)
        {
            using (var context = new AppDbContext())
            {
                var _obj = await context.Permissions.SingleOrDefaultAsync(x => x.Id == id);
                return _obj;
            }
        }
        public async Task<Permission> Update(Permission _obj)
        {
            using (var context = new AppDbContext())
            {
                var row = await context.Permissions.SingleOrDefaultAsync(x => x.Id == _obj.Id);
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
                var row = await context.Permissions.FindAsync(id);
                if (row != null)
                {
                    context.Permissions.Remove(row);
                    await context.SaveChangesAsync();
                }
                return row != null;
            }
        }
        public async Task<RolePermission?> GetRolePermission(int role_id, int permission_id)
        {
            using (var context = new AppDbContext())
            {
                var role_permission = await context.RolePermissions.SingleOrDefaultAsync(x=>x.RoleId==role_id &&x.PermissionId==permission_id);
                return role_permission;
            }
        }
        public async Task<RolePermission> AddRolePermission(RolePermission _obj)
        {
            using (var context = new AppDbContext())
            {
                context.RolePermissions.Add(_obj);
                await context.SaveChangesAsync();
                return _obj;
            }
        }
        public async Task<bool> DeleteRolePermission(int role_id, int permission_id)
        {
            using (var context = new AppDbContext())
            {
                var row = await GetRolePermission(role_id, permission_id);
                if (row != null)
                {
                    context.RolePermissions.Remove(row);
                    await context.SaveChangesAsync();
                }
                return row != null;
            }
        }
    }
}
