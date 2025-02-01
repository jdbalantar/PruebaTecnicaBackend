using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage = "El campo no tiene un formato válido")]
        [StringLength(50, ErrorMessage = "El campo no puede tener más de 50 caracteres.")]

        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [MaxLength(100, ErrorMessage = "La contraseña no puede tener más de 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{6,100}$", ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un carácter especial")]
        public required string Password { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(50, ErrorMessage = "El campo  no puede tener más de {1} caracteres")]
        [MinLength(2, ErrorMessage = "El campo no puede tener menos de {1} caracteres")]
        public required string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(50, ErrorMessage = "El campo no puede tener más de {1} caracteres")]
        [MinLength(2, ErrorMessage = "El campo no puede tener menos de {1} caracteres")]
        public required string LastName { get; set; }

        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(50, ErrorMessage = "El campo no puede tener más de {1} caracteres")]
        [MinLength(2, ErrorMessage = "El campo no puede tener menos de {1} caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "El campo solo puede contener letras, números, guiones bajos, guiones y puntos")]
        public required string UserName { get; set; }

        [Display(Name = "Número de identificación")]
        [Required(ErrorMessage = "El campo es requerido")]
        public required string Identification { get; set; }
    }
}
