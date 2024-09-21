using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.Helpers;

namespace ProjectManagementApi.DataDB
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<TaskMember> TaskMembers { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<RecurrencePattern> RecurrencePatterns { get; set; }

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=project_management.db");  // Database file
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial data
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, UserName = "Ahmed", UserEmail = "ahmed@example.com", UserPhone = "99887766", Password = MyHelper.SHA1Encode("000000") },
                new User { UserId = 2, UserName = "Ali", UserEmail = "ali@example.com", UserPhone = "77665544", Password = MyHelper.SHA1Encode("000000") },
                new User { UserId = 3, UserName = "Salim", UserEmail = "salim@example.com", UserPhone = "77889966", Password = MyHelper.SHA1Encode("000000") }
            );
            modelBuilder.Entity<Project>().HasData(
                   new Project { ProjectId = 1, ProjectTitle = "Project A", ProjectDescription = "Description A", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Status = "Active" },
                   new Project { ProjectId = 2, ProjectTitle = "Project B", ProjectDescription = "Description B", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(5), Status = "Completed" }
               );

            modelBuilder.Entity<ProjectTask>().HasData(
                   new ProjectTask { TaskId = 1, TaskTitle = "Task 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Priority = "Low", Status = "In Progress", ProjectId = 1, IsRecurring = false, TaskOwnerId = 1 },
                    new ProjectTask { TaskId = 2, TaskTitle = "Task 2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Priority = "High", Status = "Completed", ProjectId = 1, IsRecurring = false, TaskOwnerId = 2 },
                     new ProjectTask { TaskId = 3, TaskTitle = "Task 3", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Priority = "Medium", Status = "In Progress", ProjectId = 1, IsRecurring = false, TaskOwnerId = 3 },

                      new ProjectTask { TaskId = 4, TaskTitle = "Task 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Priority = "Medium", Status = "In Progress", ProjectId = 2, IsRecurring = false, TaskOwnerId = 2 },
                       new ProjectTask { TaskId = 5, TaskTitle = "Task 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Priority = "Low", Status = "In Progress", ProjectId = 2, IsRecurring = false, TaskOwnerId = 1 }
               );

            modelBuilder.Entity<TaskMember>().HasData(
                new TaskMember { TaskMemberId = 1, TaskId = 1, UserId = 1 },
                new TaskMember { TaskMemberId = 2, TaskId = 1, UserId = 2 },

                new TaskMember { TaskMemberId = 3, TaskId = 2, UserId = 2 },
                new TaskMember { TaskMemberId = 4, TaskId = 2, UserId = 3 },

                new TaskMember { TaskMemberId = 5, TaskId = 3, UserId = 3 },
                new TaskMember { TaskMemberId = 6, TaskId = 3, UserId = 2 },

                new TaskMember { TaskMemberId = 7, TaskId = 4, UserId = 2 },

                new TaskMember { TaskMemberId = 8, TaskId = 5, UserId = 1 }

                );

            modelBuilder.Entity<RecurrencePattern>().HasData(//Daily, Weekly, Monthly
                new RecurrencePattern { RecurrencePatternId=1, Pattern= "Daily", DaysCount=1 },
                new RecurrencePattern { RecurrencePatternId=2, Pattern= "Weekly", DaysCount = 7 },
                new RecurrencePattern { RecurrencePatternId=3, Pattern= "Monthly", DaysCount = 30 },
                new RecurrencePattern { RecurrencePatternId=4, Pattern= "Annually", DaysCount = 365 }
            );
        }
    }
}
