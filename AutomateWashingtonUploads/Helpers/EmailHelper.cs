﻿using AutomateWashingtonUploads.StaticData;
using System;
using System.Net.Mail;

namespace AutomateWashingtonUploads.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        ILoginInfo _info;
        ILogger _logger;

        public EmailHelper(ILoginInfo info, ILogger logger)
        {
            _info = info;
            _logger = logger;
        }

        public void SendEmail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpClient = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(_info.MailerAddress);
                mail.To.Add(_info.EmailRecipient);
                mail.Subject = $"Washington Uploads: {DateTime.Now}";
                mail.Body = "mail with attachment";

                Attachment attachment;
                attachment = new Attachment(_logger.StreamLocation);
                mail.Attachments.Add(attachment);

                SmtpClient.Port = 587;
                SmtpClient.Credentials = new System.Net.NetworkCredential(_info.MailerAddress, _info.MailerPassword);
                SmtpClient.EnableSsl = true;

                SmtpClient.Send(mail);
                Console.WriteLine("Email sent");
                SmtpClient.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, new Completion());
                Console.WriteLine("Could not send email");
            }
        }
    }
}
