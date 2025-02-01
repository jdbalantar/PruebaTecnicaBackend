using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Identification { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public virtual Student? Student { get; set; }
        public virtual Teacher? Teacher { get; set; }
    }
}
