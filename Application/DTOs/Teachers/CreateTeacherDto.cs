using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Teachers
{
    public class CreateTeacherDto
    {
        [Required(ErrorMessage = "Los nombres son requeridos")]
        [StringLength(50, ErrorMessage = "Los nombres no pueden exceder los 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Los nombres solo pueden contener letras")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [StringLength(50, ErrorMessage = "Los apellidos no pueden exceder los 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Los apellidos solo pueden contener letras")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]
        [StringLength(10, ErrorMessage = "La identificación no puede exceder los 10 caracteres")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La identificación debe tener 10 dígitos")]
        public required string Identification { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [StringLength(50, ErrorMessage = "El email no puede exceder los 50 caracteres")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
    }
}
