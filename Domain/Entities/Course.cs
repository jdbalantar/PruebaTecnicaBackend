using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Courses")]
    public class Course : AuditableEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int TeacherId { get; set; }

        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
    }
}
