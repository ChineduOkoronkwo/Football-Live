namespace Dal.AcceptanceTests.Models
{
    public class Currency : BaseModelId
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Territory { get; set; } = default!;
    }
}