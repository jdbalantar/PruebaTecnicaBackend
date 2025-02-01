using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Qualifications
{
    public class UpdateQualificationDto
    {
        [Required(ErrorMessage = "El id del estudiante es requerido")]
        public required int StudentId { get; set; }

        [Required(ErrorMessage = "La calificación es requerida")]
        public required decimal Score { get; set; }
    }
}
