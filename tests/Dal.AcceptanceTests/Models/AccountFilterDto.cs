namespace Dal.AcceptanceTests.Models
{
    public class AccountFilterDto : PaginationDto
    {
        public int? AccountTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public int? CustomerId { get; set; }
    }
}