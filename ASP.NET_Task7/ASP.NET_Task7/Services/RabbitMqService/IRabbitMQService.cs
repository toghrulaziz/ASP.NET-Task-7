namespace ASP.NET_Task7.Services.RabbitMqService
{
    public interface IRabbitMQService
    {
        void Publish<T>(T message, string queue);
    }
}
