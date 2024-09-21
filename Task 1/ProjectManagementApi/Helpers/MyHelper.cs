namespace ProjectManagementApi.Helpers
{
    public class MyHelper
    {
        public static string SHA1Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static void SendMail(string email, string subject, string body)
        {
            /*
            var builder = new BodyBuilder();
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Project Management System", "no-reply@example.com"));
            mail.To.Add(new MailboxAddress("", email));

            mail.Subject = subject;
            builder.HtmlBody = body;
            mail.Body = builder.ToMessageBody();
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("mail.example.com", 465, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");// ' Do not use OAUTH2
                    client.Authenticate("no-reply@example.com", "password");// ' Use a username / password to authenticate.
                    client.Send(mail);
                    client.Disconnect(true);
                }
                //return true;
            }
            catch (Exception ex)
            {
                //Error, could not send the message
                //Console.Write(ex.Message);
                //eventLog1.WriteEntry("SMTP Exception :" + ex.ToString());
                //return false;
            }
            */
        }
    }
}
