using ASP.NET_Task7.Data;
using ASP.NET_Task7.Models.Entities;
using ASP.NET_Task7.Providers;
using ASP.NET_Task7.Services.MailService;
using ASP.NET_Task7.Services.TodoServices;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7.Services.NatificationService
{
    public class TodoDeadlineNotificationService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public TodoDeadlineNotificationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndSendDeadlineNotifications(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); 
            }
        }

        private async Task CheckAndSendDeadlineNotifications(CancellationToken stoppingToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                var todoService = scope.ServiceProvider.GetRequiredService<ITodoService>();


                var upcomingDeadlines = await todoService.GetItemsWithUpcomingDeadlinesAsync();

                

                foreach (var deadline in upcomingDeadlines)
                {
                    var user = await todoService.GetUserByIdAsync(deadline.UserId);
                    
                    if (user != null)
                    {
                        string toAddress = user.Email!;
                        string subject = "Deadline";
                        string body = $"Hi {user.UserName}, the deadline for your task {deadline.Text} is approaching";

                        await mailService.SendMail(toAddress, subject, body);
                    }
                }


            }
        }


    }
}
