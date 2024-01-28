namespace Dal.AcceptanceTests.Models
{
    public class BaseModel : BaseModelId
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}