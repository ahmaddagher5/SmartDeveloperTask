using RBAC.Helpers;
using RBAC.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RBAC.DataDB
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserEmail { set; get; }
        public string UserPhone { set; get; }
        public int RoleId { get; set; }
        //public Role Role { get; set; }

        public User() { }
        public User(RegisterObj obj)
        {
            UserName = obj.UserName;
            Password = MyHelper.SHA1Encode(obj.Password);
            UserEmail = obj.UserEmail;
            UserPhone = obj.UserPhone;
        }
    }
}
