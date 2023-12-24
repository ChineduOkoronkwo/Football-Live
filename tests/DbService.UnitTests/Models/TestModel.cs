using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbService.UnitTests.Models
{
    public class TestModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTimeOffset DateOfBirth { get; set; } = default!;
    }
}