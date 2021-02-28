using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ConnectUs.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute("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", subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("v-connect102@outlook.com", "v-connect102@outlook.com"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            
            // var from = new EmailAddress(Options.SendGridUser, Options.SendGridUser);
            // var to  = new EmailAddress(email, email);
            // var msg= MailHelper.CreateSingleEmail(from, to, subject, message, message);

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
        

            var response =  await client.SendEmailAsync(msg);
        
            return;
        }
    }
}