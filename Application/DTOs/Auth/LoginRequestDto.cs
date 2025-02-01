using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La password es requerida")]
        [MinLength(6, ErrorMessage = "La password debe tener al menos 6 caracteres")]
        [MaxLength(100, ErrorMessage = "La password no puede tener más de 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,100}$", ErrorMessage = "La password debe tener al menos una mayúscula, una minúscula y un número")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
