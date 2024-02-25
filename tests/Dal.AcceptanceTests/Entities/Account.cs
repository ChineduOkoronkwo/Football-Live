namespace Dal.AcceptanceTests.Entities
{
    public class Account : BaseEntity
    {
        public Currency Currency { get; set; } = default!;
        public AccountType? AccountType { get; set; } = default!;
        public Customer Customer { get; set; } = default!;
        public double Balance { get; set; }
    }
}