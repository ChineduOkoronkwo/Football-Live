using DbService.Interfaces;
using DbService.Services;
using DbService.UnitTests.Models;

namespace DbService.UnitTests
{
    public class RepositoryTests
    {
        private readonly IRepository<TestEntityModel> _repository;
        public RepositoryTests()
        {
            _repository = new Repository<TestEntityModel, TestEntityIdModel>();
        }

        [Fact]
        public void GetSqlCommandSuccess()
        {
            var expected = "SELECT Name, Type, Category, Price, Id FROM TestEntityModel WHERE Id = @Id;";
            var result = _repository.GetSqlCommand;
            Assert.Equal(expected, result);
        }
    }
}