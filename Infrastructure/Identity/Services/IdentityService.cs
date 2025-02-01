using Application.DTOs.Settings;
using Application.DTOs;
using Application.Enums;
using Application.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.Auth;
using Application.Transversal;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Domain.Entities;

namespace Infrastructure.Identity.Services
{
    public class IdentityService(UserManager<User> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<User> signInManager, ILogger<IdentityService> logger) : IIdentityService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ILogger<IdentityService> logger = logger;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<Result<LoginResponseDto>> GetTokenAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                logger.LogWarning("No se encontró ninguna cuenta asociada al email {Email}", request.Email);
                return Result<LoginResponseDto>.BadRequest($"No se encontró ninguna cuenta asociada al email {request.Email}");
            }

            // Verificar si el usuario está bloqueado
            if (await _userManager.IsLockedOutAsync(user))
            {
                logger.LogWarning("La cuenta está bloqueada para {Email}", request.Email);
                return Result<LoginResponseDto>.BadRequest($"La cuenta está bloqueada. Por favor, contacte al administrador del sistema");
            }

            // Intentar iniciar sesión
            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: true);

            // Si las credenciales son incorrectas
            if (!result.Succeeded)
            {
                // Incrementar el contador de fallos
                await _userManager.AccessFailedAsync(user);

                // Si el usuario debe ser bloqueado
                if (await _userManager.IsLockedOutAsync(user))
                {
                    logger.LogWarning("La cuenta con email {Email} se ha bloqueado debido a demasiados intentos fallidos", request.Email);
                    return Result<LoginResponseDto>.BadRequest($"Demasiados intentos fallidos. Por favor, inténtelo más tarde");
                }

                logger.LogWarning("Credenciales inválidas para {Email}", request.Email);
                return Result<LoginResponseDto>.BadRequest($"Credenciales inválidas para '{request.Email}'");
            }

            // Resetear el contador de intentos fallidos después de un inicio de sesión exitoso
            await _userManager.ResetAccessFailedCountAsync(user);

            // Generar el JWT si las credenciales son correctas
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var response = new LoginResponseDto
            {
                Id = user.Id.ToString(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                IssuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
                ExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                IsVerified = user.EmailConfirmed,
                Roles = [.. rolesList]
            };

            logger.LogInformation("Inicio de sesión exitoso para {Email}", request.Email);
            return Result<LoginResponseDto>.Ok("Inicio de sesión exitoso", response);
        }


        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var tokenId = Guid.NewGuid().ToString();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("uid", user.Id.ToString()),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.LastName),
                new Claim("full_name", $"{user.FirstName} {user.LastName}"),
                new Claim("tokenId", user.SecurityStamp ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims);

            await _userManager.UpdateAsync(user);
            return JWTGeneration(claims);
        }

        private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }


        public async Task<Result<string>> RegisterAsync(RegisterUserDto request, string origin)
        {
            using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                logger.LogWarning("Ya existe una cuenta con el userName {UserName}", request.UserName);
                return Result<string>.BadRequest($"Ya existe una cuenta con el userName {request.UserName}.");
            }
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Identification = request.Identification,
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    transaction.Complete();
                    logger.LogInformation($"Usuario registrado");
                    return Result<string>.Ok($"Usuario registrado");
                }
                else
                {
                    logger.LogWarning("No se pudo registrar el usuario. Errores: {Errores}", result.GetErrorResult());
                    return Result<string>.Error("No se pudo registrar el usuario", result.GetErrorResult());
                }
            }
            else
            {
                logger.LogWarning("Ya existe una cuenta con el email {Email}", request.Email);
                return Result<string>.BadRequest($"Ya existe una cuenta con el email {request.Email}");
            }
        }


    }
}
