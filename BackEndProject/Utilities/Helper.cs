using System.IO;
using System.Net;
using System.Net.Mail;

namespace BackEndProject.Utilities
{
    public static class Helper
    {
        public static void RemoveFile(string root,string folder,string image)
        {
            string path = Path.Combine(root, folder, image);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
    public static class Email
    {
        public static void Send(string fromMail,string password,string toMail,string body, string subject)
        {
            using(var client=new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential(fromMail, password);
                client.EnableSsl = true;
                var msg = new MailMessage(fromMail, toMail);
                msg.Body = body;
                msg.Subject = subject;
                msg.IsBodyHtml = true;
                client.Send(msg);
            }
        }
    }
    public enum UserRoles
    {
        Admin,
        Member
    }
}
