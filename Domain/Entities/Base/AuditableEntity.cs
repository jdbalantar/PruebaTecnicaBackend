using Domain.Interfaces;

namespace Domain.Entities.Base
{
    public abstract class AuditableEntity : IAuditableBaseEntity, IBaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
    }
}
