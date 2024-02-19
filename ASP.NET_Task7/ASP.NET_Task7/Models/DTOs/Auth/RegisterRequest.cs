namespace ASP.NET_Task7.Models.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
