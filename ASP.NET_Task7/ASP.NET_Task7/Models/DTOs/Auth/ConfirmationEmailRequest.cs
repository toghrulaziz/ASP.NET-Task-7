namespace ASP.NET_Task7.Models.DTOs.Auth
{
    public class ConfirmationEmailRequest
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
