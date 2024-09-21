using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApi.DataDB
{
    public class TaskLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskLogId { get; set; }
        public int TaskId { get; set; }
        public string old_status { get; set; }
        public string new_status { get; set; }
        public int created_by { get; set; }
        public DateTime created_at { get; set; }
    }
}
