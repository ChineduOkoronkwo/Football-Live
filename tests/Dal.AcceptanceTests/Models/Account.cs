namespace Dal.AcceptanceTests.Models
{
    public class Account : BaseModel
    {
        public int CurrencyId { get; set; }
        public int AccountTypeId { get; set; }
        public int CustomerId { get; set; }
        public double Balance { get; set; }
    }
}