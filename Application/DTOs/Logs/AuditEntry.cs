using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Application.DTOs.Logs
{
    public class AuditEntry(EntityEntry entry)
    {
        public EntityEntry Entry { get; } = entry;

        public required string UserId { get; set; }

        public required string TableName { get; set; }

        public Dictionary<string, object> KeyValues { get; } = [];


        public Dictionary<string, object> OldValues { get; } = [];


        public Dictionary<string, object> NewValues { get; } = [];


        public List<PropertyEntry> TemporaryProperties { get; } = [];


        public AuditType AuditType { get; set; }

        public List<string> ChangedColumns { get; } = [];

        public bool HasTemporaryProperties => TemporaryProperties.Count != 0;
        public required string ControllerName { get; set; } = string.Empty;
        public required string ActionName { get; set; } = string.Empty;

        public Audit ToAudit()
        {
            return new Audit
            {
                UserId = UserId,
                Type = AuditType.ToString(),
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                PrimaryKey = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns),
                ControllerName = ControllerName,
                ActionName = ActionName
            };
        }
    }
}
