using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("Students")]
    public class Student : AuditableEntity
    {
        [Required(ErrorMessage = "El id del usuario es requerido")]
        public int UserId { get; set; }
        public required int CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }
    }
}
