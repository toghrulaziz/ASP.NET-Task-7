using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokerConsumer.Configurations
{
    public class RabbitMQConfiguration
    {
        public string HostName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int Port { get; set; }
        public string QueueName { get; set; } = default!;
    }
}
