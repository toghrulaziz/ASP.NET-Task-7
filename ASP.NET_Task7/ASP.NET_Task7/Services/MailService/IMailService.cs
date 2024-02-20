namespace ASP.NET_Task7.Services.MailService
{
    public interface IMailService
    {
        Task SendMail(string to, string subject, string body);
    }
}
