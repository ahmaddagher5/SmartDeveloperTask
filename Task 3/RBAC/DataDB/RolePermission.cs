using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RBAC.DataDB
{
    public class RolePermission
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RoleId { get; set; }
        //public Role Role { get; set; }

        public int PermissionId { get; set; }
        //public Permission Permission { get; set; }
    }
}
