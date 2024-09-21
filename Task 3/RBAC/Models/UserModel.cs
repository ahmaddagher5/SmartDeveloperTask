using Microsoft.EntityFrameworkCore;
using RBAC.DataDB;

namespace RBAC.Models
{
    public class UserModel
    {
        public async Task<User?> Login(string userphone, string password)
        {
            using (var context = new AppDbContext())
            {
                var obj = await context.Users.SingleOrDefaultAsync(x => x.UserPhone.Equals(userphone) && x.Password == password);
                return obj;
            }
        }
        public async Task<bool> IsUserPhoneExisting(string userphone)
        {
            using (var context = new AppDbContext())
            {
                var obj = await context.Users.SingleOrDefaultAsync(x => x.UserPhone.Equals(userphone));
                return obj != null;
            }
        }
        public async Task<User?> GetById(int id)
        {
            using (var context = new AppDbContext())
            {
                var _obj = await context.Users.SingleOrDefaultAsync(x => x.UserId == id);
                return _obj;
            }
        }
        public async Task<User> Register(User new_user)
        {
            using (var context = new AppDbContext())
            {
                context.Users.Add(new_user);
                await context.SaveChangesAsync();
                return new_user;
            }

        }
        public async Task<User> AssignRoleToUser(int userId, int roleId)
        {
            using (var context = new AppDbContext())
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserId == userId);
                if (user != null)
                {
                    user.RoleId = roleId;
                    await context.SaveChangesAsync();
                }
                return user;
            }
        }
        public async Task<bool> CheckUserPermission(int userId, string permission)
        {
            using (var context = new AppDbContext()) { 
                var userPermission = await context.Users.Where(x=>x.UserId == userId)
                                           .Join(context.Roles, u=>u.RoleId, r=>r.Id, (u, r) => new { u,r })
                                           .Join(context.RolePermissions, ur=>ur.r.Id, rp=>rp.RoleId, (ur, rp)=>new { ur, rp })
                                           .Join(context.Permissions, urup=>urup.rp.PermissionId, p=>p.Id, (urup, p)=>new { p.Name })
                                           .Where(p=>p.Name==permission)
                                           .FirstOrDefaultAsync();
                return userPermission != null;
            }
        }
    }
}
