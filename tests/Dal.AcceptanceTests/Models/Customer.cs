namespace Dal.AcceptanceTests.Models
{
    public class Customer : BaseModelId
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Middlename { get; set; } = default!;
        public DateTime DateOfBirth { get; set; } = default!;
    }
}