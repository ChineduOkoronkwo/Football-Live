using System.Globalization;
using Dal.UnitTests.Entities;

namespace Dal.UnitTests.Utils
{
    public class TestUtil
    {
        protected const string _listSqlCommand = "SELECT * FROM TestEntity WHERE FirstName=@FirstName ORDER BY DateOfBirth DESC Limit @PageSize OFFSET @Offset;";
        protected const string _getSqlCommand = "SELECT * FROM TestEntity WHERE Id=@Id;";
        protected const string _createSqlCommand = "INSERT INTO TestEntity(Id, FirstName, LastName, DateOfBirth) VALUES(@Id, @FirstName, @LastName, @DateOfBirth);";
        protected const string _updateSqlCommand = "UPDATE TestEntity SET FirstName=@FirstName, LastName=@LastName, DateOfBirth=@DateOfBirth WHERE Id=@Id;";
        protected const string _deleteSqlCommand = "Delete FROM TestEntity WHERE Id=@Id;";
        protected const string _testExceptionMessage = "Test Exception Message";
        protected TestEntity Person1 { get; }
        protected List<TestEntity> PersonList { get; }
        protected EntityId EntityIdParam { get; }
        public TestUtil()
        {
            Person1 = new TestEntity()
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                DateOfBirth = DateTime.Parse("1957-02-06", new CultureInfo("en-US")),
            };
            PersonList =
            [
                Person1,
                new TestEntity()
                {
                    FirstName = "Test FirstName2",
                    LastName = "Test LastName2",
                    DateOfBirth = DateTime.Parse("1995-12-25", new CultureInfo("en-US")),
                }

            ];
            EntityIdParam = new EntityId { Id = 1 };
        }
    }
}