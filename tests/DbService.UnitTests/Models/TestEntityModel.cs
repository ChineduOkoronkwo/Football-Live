namespace DbService.UnitTests.Models
{
    public class TestEntityModel : TestIdEntityModel
    {
        required public string Name { get; set; }
        required public string Type { get; set; }
        required public string Category { get; set; }
        public decimal Price { get; set; }
    }
}