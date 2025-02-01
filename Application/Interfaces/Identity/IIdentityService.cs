using Application.DTOs.Auth;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<Result<LoginResponseDto>> GetTokenAsync(LoginRequestDto request);
        Task<Result<string>> RegisterAsync(RegisterUserDto request, string origin);
    }
}
