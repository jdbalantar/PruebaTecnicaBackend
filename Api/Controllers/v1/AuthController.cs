using Application.DTOs.Auth;
using Application.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiController]
    public class AuthController(IIdentityService identityService) : BaseApiController<AuthController>
    {
        private readonly IIdentityService identityService = identityService;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginRequestDto tokenRequest)
        {
            var result = await identityService.GetTokenAsync(tokenRequest);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto requiest)
        {
            var result = await identityService.RegisterAsync(requiest, string.Empty);
            return StatusCode(result.StatusCode, result);
        }


    }
}
