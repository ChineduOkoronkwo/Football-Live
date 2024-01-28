namespace Dal.AcceptanceTests.Models
{
    public class Account : BaseModel
    {
        public string CurrencyCode { get; set; } = default!;
        public Guid AccountTypeId { get; set; }
        public Guid CustomerId { get; set; }
        public double Balance { get; set; }
    }
}