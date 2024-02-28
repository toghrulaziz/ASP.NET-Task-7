using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace ASP.NET_Task7.Services.RabbitMqService
{
    public class RabbitMQService(IConnectionFactory connectionFactory) : IRabbitMQService
    {
        public void Publish<T>(T message, string queueName)
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
