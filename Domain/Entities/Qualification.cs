using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Qualifications")]
    public class Qualification : AuditableEntity
    {
        public required int StudentId { get; set; }

        [Column(TypeName = "decimal(5,1)")]
        [Range(0.0, 10.0, ErrorMessage = "La calificación debe estar entre 0 y 10")]
        public required decimal Score { get; set; }
        public virtual Student? Student { get; set; }
    }
}
