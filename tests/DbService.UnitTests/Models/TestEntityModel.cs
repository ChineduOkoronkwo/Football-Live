namespace DbService.UnitTests.Models
{
    public class TestEntityModel : TestEntityIdModel
    {
        required public string Name { get; set; }
        required public string Type { get; set; }
        public int Category { get; set; }
        public decimal Price { get; set; }
    }
}