using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.DataDB;
using ProjectManagementApi.Helpers;

namespace ProjectManagementApi.Models
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
            using (var context = new AppDbContext()) { 
                context.Users.Add(new_user);
                await context.SaveChangesAsync();
                return new_user;
            }

        }
        
    }
}
