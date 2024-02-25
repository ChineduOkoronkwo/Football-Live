namespace Dal.AcceptanceTests.Entities
{
    public class Currency : EntityId
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Territory { get; set; } = default!;
    }
}