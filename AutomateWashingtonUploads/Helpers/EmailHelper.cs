using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AutomateWashingtonUploads.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        ILogger _logger;

        public EmailHelper(ILogger logger)
        {
            _logger = logger;
        }

        public async Task SendEmail(IConfiguration config)
        {
            var recipients = new List<Address>();
            Console.WriteLine(config["emailRecipients"]);

            foreach (var emailAdress in config.GetSection("emailRecipients").Value.Split(","))
            {
                recipients.Add(new Address(emailAdress));
            }

            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                Port = 587,
                Credentials = new System.Net.NetworkCredential(config.GetSection("mailerAddress").Value, config.GetSection("mailerPassword").Value)
            });

            Email.DefaultSender = sender;

            var email = await Email
                .From(config.GetSection("mailerAddress").Value)
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
