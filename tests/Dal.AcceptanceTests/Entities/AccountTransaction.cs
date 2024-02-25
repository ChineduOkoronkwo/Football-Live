namespace Dal.AcceptanceTests.Entities
{
    public class AccountTransaction : BaseEntity
    {
        public int AccountId { get; set; }
        public double Amt { get; set; }
        public int CurrencyId { get; set; } = default!;
        public int? TransactionTypeId { get; set; }
        public System.DateTime TransDate { get; set; }
    }
}