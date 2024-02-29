using MessageBrokerConsumer.Configurations;
using MessageBrokerConsumer.DTOs;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using MessageBrokerConsumer.Services.MailService;


string FILE_NAME = @"C:\Users\ASUS\Desktop\ASP.NET Tasks\ASP.NET Task 7\ASP.NET_Task7\MessageBrokerConsumer\config.json";


var configuration = new ConfigurationBuilder()
            .AddJsonFile(FILE_NAME)
            .Build();

var serviceProvider = new ServiceCollection()
            .AddSingleton<IMailService>(provider => new MailService(
                configuration["Smtp:Host"]!,
                configuration.GetValue<int>("Smtp:Port"),
                configuration["Smtp:Username"]!,
                configuration["Smtp:Password"]!,
                configuration["Smtp:FromAddress"]!
            ))
            .BuildServiceProvider();

var mailService = serviceProvider.GetRequiredService<IMailService>();

var rabbitMQConfig = new RabbitMQConfiguration();
configuration.Bind("RabbitMQ", rabbitMQConfig);

var factory = new ConnectionFactory
{
    HostName = rabbitMQConfig.HostName,
    UserName = rabbitMQConfig.UserName,
    Password = rabbitMQConfig.Password,
    Port = rabbitMQConfig.Port,
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: rabbitMQConfig.QueueName,
                     exclusive: false,
                     durable: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);


consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var confirmationMessage = JsonSerializer.Deserialize<ConfirmationMessageDto>(message);

    string confirmationLink = $"https://localhost:7247/api/auth/email-confirm?email={confirmationMessage!.Email}&token={confirmationMessage!.RefreshToken}";

    Console.WriteLine(confirmationMessage.RefreshToken);

    string toAddress = confirmationMessage!.Email!;
    string subject = "Confirmation Email";
    //string mailbody = $"Dear User,<br><br>" +
    //           "Thank you for registering.";

    string mailbody = $"Dear {confirmationMessage.Username},<br><br>" +
                  "Thank you for registering. Please click the following link to confirm your email address:<br>" +
                  $"<a href='{confirmationLink}'>Confirm Email Address</a>";


    mailService.SendMail(toAddress, subject, mailbody);
};

channel.BasicConsume(queue: rabbitMQConfig.QueueName,
                    autoAck: true,
                    consumer: consumer);
Console.WriteLine("Press [enter] to exit");
Console.ReadLine();