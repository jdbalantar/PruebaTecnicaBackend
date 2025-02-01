using System.Text.Json.Serialization;

namespace Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public required string Token { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
