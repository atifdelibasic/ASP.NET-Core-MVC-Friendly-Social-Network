using FriendlyRS1.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridEmailSender(IOptions<EmailSenderOptions> options)
        {
            this.Options = options.Value;
        }

        public EmailSenderOptions Options { get; set; }

        public Task SendEmailAsync(string email, string subject, string url, string message)
        {
            return Execute(Options.SendGridKey, email, subject, url, message);
        }

        public Task Execute(string apiKey, string to, string subject, string url, string message)
        {
            var client = new SendGridClient(apiKey);
            const string quote = "\"";

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SenderEmail, Options.SendGridName),
                Subject = subject,
                HtmlContent = message + "<a href=" + quote + url + quote + "> Link</a>.<br><br> Friendly Team"
            };
            msg.AddTo(new EmailAddress(to));

            // Disable click tracking.
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
