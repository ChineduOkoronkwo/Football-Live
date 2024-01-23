namespace Dal.UnitTests.Entities
{
    public class TestEntity : EntityId
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTimeOffset DateOfBirth { get; set; } = default!;
    }
}