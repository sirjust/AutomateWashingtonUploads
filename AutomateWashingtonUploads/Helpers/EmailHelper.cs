using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using AutomateWashingtonUploads.StaticData;

namespace AutomateWashingtonUploads.Helpers
{
    public static class EmailHelper
    {
        public static void SendEmail(StreamReader reader, LoginInfo info)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(info.MailerAddress);
                mail.To.Add(info.EmailRecipient);
                mail.Subject = $"Washington Uploads: {DateTime.Today}";
                mail.Body = "mail with attachment";

                Attachment attachment;
                attachment = new Attachment(Logger.StreamLocation);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(info.MailerAddress, info.MailerPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Email sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
