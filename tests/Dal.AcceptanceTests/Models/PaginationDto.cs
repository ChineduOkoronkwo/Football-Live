namespace Dal.AcceptanceTests.Models
{
    public class PaginationDto
    {
        public int PageSize { get; set; } = 100;
        public int PageOffset { get; set; } = 0;
    }
}