using System;
using System.Net;
using System.Net.Mail;

namespace EmailSenderClient
{
    public static class EmailSender
    {
        public static bool Send(string to, string message)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                NetworkCredential credential = new NetworkCredential("acanakdasdev@gmail.com", "bns80acanakdas");
                smtpClient.Credentials = credential;
                MailAddress sender = new MailAddress("acanakdasdev@gmail.com", "SignalR-RabbitMQ-Workspace");
                MailAddress receiver = new MailAddress(to);
                MailMessage mail = new MailMessage(sender, receiver);
                mail.Subject = "SignalR-RabbitMQ-Project";
                mail.Body = message;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}