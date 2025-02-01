namespace Domain.Entities
{
    public class Audit
    {
        public int Id { get; set; }

        public string? UserId { get; set; } = string.Empty;

        public string? Type { get; set; } = string.Empty;

        public string? TableName { get; set; } = string.Empty;

        public DateTime DateTime { get; set; }

        public string? OldValues { get; set; } = string.Empty;

        public string? NewValues { get; set; } = string.Empty;

        public string? AffectedColumns { get; set; } = string.Empty;

        public string? PrimaryKey { get; set; } = string.Empty;
        public string? ControllerName { get; set; } = string.Empty;
        public string? ActionName { get; set; } = string.Empty;
    }
}
