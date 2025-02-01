using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{

    public class Teacher : AuditableEntity
    {
        [Required(ErrorMessage = "El id del usuario es requerido")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Course>? Courses { get; set; }
    }
}
