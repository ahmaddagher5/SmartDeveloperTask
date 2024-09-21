using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RBAC.DataDB
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } // e.g., Admin, User
        //public ICollection<RolePermission> RolePermissions { get; set; }
        //public ICollection<User> Users { get; set; }
    }
}
