namespace Dal.UnitTests.Entities
{
    public class ListParamEntity
    {
        required public string FirstName { get; set; }
        public int PageSize { get; set; } = 100;
        public int Offset { get; set; }
    }
}