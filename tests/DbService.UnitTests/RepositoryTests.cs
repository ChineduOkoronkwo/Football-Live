using DbService.Interfaces;
using DbService.Services;
using DbService.UnitTests.Models;
using Moq;

namespace DbService.UnitTests
{
    public class RepositoryTests
    {
        private const string GetSqlCommand = "SELECT Name, Type, Category, Price, Id FROM TestEntityModel WHERE Id = @Id;";
        private readonly Mock<IQueryService> _mockQueryService;
        public RepositoryTests()
        {
            _mockQueryService = new Mock<IQueryService>();
        }

        [Fact]
        public async Task GetAsyncReturnsSingleItem()
        {
            var repository = new Repository<TestEntityModel, TestIdEntityModel>(_mockQueryService.Object);
            var testIdEntity = new TestIdEntityModel { Id = 10 };
            var testEntity = new TestEntityModel
            {
                Id = 10,
                Name = "Blue Ray Shoe",
                Category = "Foot Wear",
                Price = 20.50M,
                Type = "Shoe"
            };
            _mockQueryService.Setup(
                q => q.QuerySingleAsync<TestEntityModel>(
                It.IsAny<string>(),
                It.IsAny<object>())).ReturnsAsync(testEntity);
            var result = await repository.GetAsync(testIdEntity, false);
            Assert.Equal(testEntity, result);
            _mockQueryService.Verify(q => q.QuerySingleAsync<TestIdEntityModel>(GetSqlCommand, testIdEntity));
        }
    }
}