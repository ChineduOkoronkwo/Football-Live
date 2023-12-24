using System.Data;
using System.Globalization;
using Dapper;
using DbService.Interfaces;
using DbService.Services;
using DbService.UnitTests.Models;
using Moq;
using Moq.Dapper;

namespace DbService.UnitTests
{
    public class QueryServiceTests
    {
        private const string _testSql = "test-sql";
        private readonly IQueryService _queryService;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly TestModel _person1;
        private readonly TestModel _person2;

        public QueryServiceTests()
        {
            _mockConnection = new ();
            _queryService = new QueryService(_mockConnection.Object);
            _person1 = new TestModel()
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                DateOfBirth = DateTime.Parse("1957-02-06", new CultureInfo("en-US")),
            };
            _person2 = new TestModel()
            {
                FirstName = "Test FirstName2",
                LastName = "Test LastName2",
                DateOfBirth = DateTime.Parse("1995-12-25", new CultureInfo("en-US")),
            };
        }

        [Fact]
        public async Task QuerySingleAsyncReturnsItemForValidCall()
        {
            _mockConnection.SetupDapperAsync(
                c => c.QuerySingleAsync<TestModel>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null)).ReturnsAsync(_person1);
            var parms = new object();
            var result = await _queryService.QuerySingleAsync<TestModel>(_testSql, parms);
            Assert.Equivalent(result, _person1);
        }

        [Fact]
        public async Task QueryAsyncReturnsListOfItemsForValidCall()
        {
            var expected = new List<TestModel> { _person1, _person2 };
            var parms = new object();
            _mockConnection.SetupDapperAsync(
                c => c.QueryAsync<TestModel>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null)).ReturnsAsync(expected);
            var result = await _queryService.QueryAsync<TestModel>(_testSql, parms);
            Assert.Equivalent(result, expected);
        }

        [Fact]
        public async Task ExecuteAsyncReturnsOneForValidCall()
        {
            var expected = 1;
            var parms = new object();
            _mockConnection.SetupDapperAsync(
                c => c.ExecuteAsync(
                    _testSql,
                    parms,
                    null,
                    null,
                    null)).ReturnsAsync(expected);
            var result = await _queryService.ExecuteAsync(_testSql, parms);
            Assert.Equivalent(result, expected);
        }
    }
}