using Application.Interfaces.Shared;
using System.Security.Claims;

namespace Api.Services
{
    public class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserService
    {
        public string UserId { get; } = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid") ?? string.Empty;
        public string Username { get; } = httpContextAccessor.HttpContext?.User?.FindFirstValue("username") ?? string.Empty;
    }
}
