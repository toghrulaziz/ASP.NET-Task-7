namespace ASP.NET_Task7.Models.DTOs.Auth
{
    public class AuthTokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
