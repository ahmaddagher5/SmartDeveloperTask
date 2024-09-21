using RBAC.DataDB;
using System.Security.Claims;
namespace RBAC.Models
{
    public class AuditLogModel
    {
        /*
         public int Id { get; set; }
        public string Action { get; set; } // e.g., "Create Role", "Delete Permission"
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; } // Track who performed the action
         */
        public static async Task CreateLog(string action, int user_id)
        {
            using(var context = new AppDbContext())
            {
                var auditLog = new AuditLog() { Action=action, UserId = user_id, Timestamp=DateTime.Now};
                context.AuditLogs.Add(auditLog);
                await context.SaveChangesAsync();
            }
        }
    }
}
