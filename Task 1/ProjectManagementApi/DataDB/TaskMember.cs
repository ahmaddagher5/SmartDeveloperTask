using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApi.DataDB
{
    public class TaskMember
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskMemberId { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
