using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Courses
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "El nombre del curso es requerido")]
        [MaxLength(20, ErrorMessage = "El nombre del curso no puede exceder los 20 caracteres")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La descripción del curso es requerida")]
        [MaxLength(500, ErrorMessage = "La descripción del curso no puede exceder los 200 caracteres")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "El id del profesor es requerido")]
        public int? TeacherId { get; set; }
    }
}
