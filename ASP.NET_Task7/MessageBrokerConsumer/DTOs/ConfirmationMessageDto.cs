using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokerConsumer.DTOs
{
    public class ConfirmationMessageDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
