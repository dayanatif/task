using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace TSManager.Services
{
    public class EmailService
    {
        private readonly EmailSettings _s;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _s = settings.Value;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            using var msg = new MailMessage
            {
                From = new MailAddress(_s.SmtpUser, _s.FromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            msg.To.Add(toEmail);

            using var smtp = new SmtpClient(_s.SmtpHost, _s.SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_s.SmtpUser, _s.SmtpPass)
            };

            await smtp.SendMailAsync(msg);
        }
    }
}
