using ASP.NET_Task7.Auth;
using ASP.NET_Task7.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Task7.Models.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Task7.Services.RabbitMqService;
using ASP.NET_Task7.Configuration;

namespace ASP.NET_Task7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService, IRabbitMQService rabbitMQService, RabbitMQConfiguration rabbitMQConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _rabbitMQService = rabbitMQService;
            _rabbitMQConfiguration = rabbitMQConfiguration;
        }

        private async Task<AuthTokenDto> GenerateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var accessToken = _jwtService.GenerateSecurityToken(user.Id, user.Email!, roles, claims);

            var refreshToken = Guid.NewGuid().ToString().ToLower();
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new AuthTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthTokenDto>> Register(Models.DTOs.Auth.RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Conflict("User already exist");

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                RefreshToken = Guid.NewGuid().ToString().ToLower(),
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var confirmationMessage = new ConfirmationMessageDto
            {
                UserId = user.Id,
                Email = user.Email
            };

            _rabbitMQService.Publish<ConfirmationMessageDto>(confirmationMessage, _rabbitMQConfiguration.QueueName);

            return await GenerateToken(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokenDto>> Login(Models.DTOs.Auth.LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return BadRequest("User Not Found");

            var canSignIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!canSignIn.Succeeded)
                return BadRequest();

            return await GenerateToken(user);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthTokenDto>> RefreshToken(Models.DTOs.Auth.RefreshRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user is null)
                return Unauthorized();

            return await GenerateToken(user);
        }
    }
}
