using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RBAC.DataDB
{
    public class AuditLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Action { get; set; } // e.g., "Create Role", "Delete Permission"
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; } // Track who performed the action
    }
}
