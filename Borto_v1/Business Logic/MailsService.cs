using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    class MailsService
    {
        public static void SendEmail(string emailTo, string title, string htmlBody = "")
        {
            using (MailMessage mail = new MailMessage())
            {
                string login = ConfigurationManager.AppSettings["emailLogin"];

                string password = ConfigurationManager.AppSettings["emailPass"];

                mail.From = new MailAddress(login);
                mail.To.Add(emailTo);
                mail.Subject = title;
                mail.Body = htmlBody;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(login, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public static string GetPass(int x)
        {
            string pass = "";
            var r = new Random();
            while (pass.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }
            return pass;
        }
    }
}
