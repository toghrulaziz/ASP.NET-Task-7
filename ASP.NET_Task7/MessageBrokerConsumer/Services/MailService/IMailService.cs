using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokerConsumer.Services.MailService
{
    public interface IMailService
    {
        Task SendMail(string to, string subject, string body);
    }
}
