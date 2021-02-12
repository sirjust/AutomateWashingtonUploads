using AutomateWashingtonUploads.StaticData;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

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

        public async Task SendEmail()
        {
            var recipients = new List<Address>();

            foreach(var emailAdress in _info.EmailRecipients)
            {
                recipients.Add(new Address(emailAdress));
            }

            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                Port = 587,
                Credentials = new System.Net.NetworkCredential(_info.MailerAddress, _info.MailerPassword)
            });

            Email.DefaultSender = sender;

            var email = await Email
                .From(_info.MailerAddress)
                .To(recipients)
                .Subject($"Washington Uploads: {DateTime.Now}")
                .Body("See attachment below for logs")
                .AttachFromFilename(_logger.StreamLocation)
                .SendAsync();

            if (email.Successful)
                Console.WriteLine("Email sent successfully");
            else Console.WriteLine("Could not send email");
        }
    }
}
