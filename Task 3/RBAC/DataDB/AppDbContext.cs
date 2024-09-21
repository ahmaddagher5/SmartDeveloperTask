
using Microsoft.EntityFrameworkCore;
using RBAC.Helpers;

namespace RBAC.DataDB
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=rbac_db.db");  // Database file
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "Ahmed", UserEmail = "ahmed@example.com", UserPhone = "99887766", Password = MyHelper.SHA1Encode("000000"), RoleId = 1 },
                new User { UserId = 2, UserName = "Ali", UserEmail = "ali@example.com", UserPhone = "77665544", Password = MyHelper.SHA1Encode("000000"), RoleId = 2 },
                new User { UserId = 3, UserName = "Salim", UserEmail = "salim@example.com", UserPhone = "77889966", Password = MyHelper.SHA1Encode("000000"), RoleId = 2 }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Role { Id = 1, Name = "Create" },
                new Role { Id = 2, Name = "Update" },
                new Role { Id = 3, Name = "Delete" },
                new Role { Id = 4, Name = "List" },
                new Role { Id = 5, Name = "View" }
            );

            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
                new RolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
                new RolePermission { Id = 3, RoleId = 1, PermissionId = 3 },
                new RolePermission { Id = 4, RoleId = 1, PermissionId = 4 },
                new RolePermission { Id = 5, RoleId = 1, PermissionId = 5 },

                new RolePermission { Id = 6, RoleId = 2, PermissionId = 4 },
                new RolePermission { Id = 7, RoleId = 2, PermissionId = 5 }
            );
        }
    }
}
