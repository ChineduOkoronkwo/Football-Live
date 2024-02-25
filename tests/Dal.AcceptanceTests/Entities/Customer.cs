using System;

namespace Dal.AcceptanceTests.Entities
{
    public class Customer
    {
        required public Guid CustomerId { get; set; }
        required public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Middlename { get; set; } = default!;
        public DateTime? DateOfBirth { get; set; } = default!;
    }
}