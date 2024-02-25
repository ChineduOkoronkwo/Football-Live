namespace Dal.AcceptanceTests.Entities
{
    public class BaseEntity : EntityId
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
    }
}