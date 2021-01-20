using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Services.Messages.Email
{
    public class EmailOptions
    {
        public const string Email = "Email";

        public string Name { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
    }

    public class EmailService: EmailSender
    {
        private readonly EmailOptions _emailOptions;
        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public override async Task SendEmailAsync(List<string> adresses, EmailMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailOptions.Name, _emailOptions.Address));
            emailMessage.To.AddRange(adresses.Select(x => new MailboxAddress("", x)).ToList());
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, _emailOptions.SSL);
                await client.AuthenticateAsync(_emailOptions.Address, _emailOptions.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
