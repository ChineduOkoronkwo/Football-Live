using System.Data;
using System.Globalization;
using Dal.Interfaces;
using Dal.Services;
using Dal.UnitTests.Entities;
using Dapper;
using Moq;
using Moq.Dapper;

namespace Dal.UnitTests;

public class DapperServiceTests
{
    private const string _testSql = "test-sql";
    private readonly IDapperService _dapperService;
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly TestEntity _person1;
    private readonly TestEntity _person2;

    public DapperServiceTests()
    {
        _mockConnection = new ();
        _dapperService = new DapperService(_mockConnection.Object);
        _person1 = new TestEntity()
        {
            FirstName = "Test FirstName",
            LastName = "Test LastName",
            DateOfBirth = DateTime.Parse("1957-02-06", new CultureInfo("en-US")),
        };
        _person2 = new TestEntity()
        {
            FirstName = "Test FirstName2",
            LastName = "Test LastName2",
            DateOfBirth = DateTime.Parse("1995-12-25", new CultureInfo("en-US")),
        };
    }

    [Fact]
    public async Task QuerySingleAsyncReturnsItemAsync()
    {
        _mockConnection.SetupDapperAsync(
                c => c.QuerySingleAsync<TestEntity>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null)).ReturnsAsync(_person1);
        var parms = new object();
        var result = await _dapperService.QuerySingleAsync<TestEntity>(_testSql, parms);
        Assert.Equivalent(result, _person1);
    }

    [Fact]
    public async Task QueryAsyncReturnsListOfItemsForValidCall()
    {
        var expected = new List<TestEntity> { _person1, _person2 };
        var parms = new object();
        _mockConnection.SetupDapperAsync(
            c => c.QueryAsync<TestEntity>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ReturnsAsync(expected);
        var result = await _dapperService.QueryAsync<TestEntity>(_testSql, parms);
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
        var result = await _dapperService.ExecuteAsync(_testSql, parms);
        Assert.Equivalent(result, expected);
    }
}