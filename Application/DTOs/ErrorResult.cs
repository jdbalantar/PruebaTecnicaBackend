namespace Application.DTOs
{
    public sealed record ErrorResult(string? Code, string? Description)
    {
        public static readonly ErrorResult None = new(null, null);
    }
}
