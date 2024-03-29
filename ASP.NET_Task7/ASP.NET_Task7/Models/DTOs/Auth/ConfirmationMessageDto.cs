﻿namespace ASP.NET_Task7.Models.DTOs.Auth
{
    public class ConfirmationMessageDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
    }
}
