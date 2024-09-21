namespace ProjectManagementApi.Objects
{
    public class RegisterObj
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserEmail { set; get; }
        public string UserPhone { set; get; }
    }
}
