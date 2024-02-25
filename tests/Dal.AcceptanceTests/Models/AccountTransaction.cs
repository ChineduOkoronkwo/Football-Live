namespace Dal.AcceptanceTests.Models
{
    public class AccountTransaction : BaseModel
    {
        public int AccountId { get; set; }
        public double Amt { get; set; }
        public int CurrencyId { get; set; } = default!;
        public int TransactionTypeId { get; set; }
    }
}