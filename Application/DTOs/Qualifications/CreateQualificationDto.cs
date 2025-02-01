using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Qualifications
{
    public class CreateQualificationDto
    {
        [Required(ErrorMessage = "El id del estudiante es requerido")]
        public required int StudentId { get; set; }

        [Required(ErrorMessage = "La calificación es requerida")]
        public required decimal Score { get; set; }
    }
}
