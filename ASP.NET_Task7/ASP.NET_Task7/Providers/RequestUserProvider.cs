namespace ASP.NET_Task7.Providers
{
    public class RequestUserProvider : IRequestUserProvider
    {
        private readonly HttpContext _context;

        public RequestUserProvider(IHttpContextAccessor accessor)
        {
            _context = accessor.HttpContext!;
        }
        public UserInfo? GetUserInfo()
        {
            if (!_context.User.Claims.Any()) return null;

            var userId = _context.User.Claims.First(e => e.Type == "userId").Value;
            var username = _context.User.Identity!.Name!;

            return new UserInfo(userId, username);
        }
    }
}
