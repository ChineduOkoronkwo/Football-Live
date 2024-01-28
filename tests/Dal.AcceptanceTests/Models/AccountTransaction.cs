namespace Dal.AcceptanceTests.Models
{
    public class AccountTransaction : BaseModel
    {
        public Guid AccountId { get; set; }
        public double Amt { get; set; }
        public string CurrencyCode { get; set; } = default!;
        public Guid TransactionTypeId { get; set; }
    }
}