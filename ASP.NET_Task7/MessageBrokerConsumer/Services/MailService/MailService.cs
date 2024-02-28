using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokerConsumer.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public MailService(string host, int port, string username, string password, string fromAddress)
        {
            _smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };
            _fromAddress = fromAddress;
        }

        public async Task SendMail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage(_fromAddress, to, subject, body);
            mailMessage.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
