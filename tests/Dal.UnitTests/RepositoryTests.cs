using Dal.Interfaces;
using Dal.Services;
using Dal.UnitTests.Entities;
using Dal.UnitTests.Utils;
using Moq;

namespace Dal.UnitTests
{
    public class RepositoryTests : TestUtil
    {
        private readonly Mock<IDapperService> _dapperService;
        private readonly Mock<IEntitySqlCommand> _entitySqlCommand;
        private readonly IRepository _repository;

        public RepositoryTests()
        {
            _dapperService = new Mock<IDapperService>();
            _entitySqlCommand = new Mock<IEntitySqlCommand>();
            _repository = new Repository(_dapperService.Object, _entitySqlCommand.Object);
        }

        [Fact]
        public async Task GetAsyncReturnsSingleItem()
        {
            _entitySqlCommand.Setup(s => s.GetSqlCommand).Returns(_getSqlCommand);
            _dapperService.Setup(d => d.QuerySingleAsync<TestEntity>(_getSqlCommand, EntityIdParam)).ReturnsAsync(Person1);

            var actual = await _repository.GetAsync<TestEntity>(EntityIdParam);

            Assert.Equal(Person1, actual);
            _dapperService.VerifyAll();
        }

        [Fact]
        public async Task GetAsyncPropagatesException()
        {
            var expectedException = new Exception(_testExceptionMessage);
            _entitySqlCommand.Setup(s => s.GetSqlCommand).Returns(_getSqlCommand);
            _dapperService.Setup(d => d.QuerySingleAsync<TestEntity>(_getSqlCommand, EntityIdParam)).ThrowsAsync(expectedException);

            async Task Act() => await _repository.GetAsync<TestEntity>(EntityIdParam);

            var actualException = await Assert.ThrowsAsync<Exception>(Act);
            Assert.Equal(expectedException, actualException);
            _dapperService.VerifyAll();
        }

        [Fact]
        public async Task ListAsyncReturnsListOfItems()
        {
            var sqlparam = new ListParamEntity
            {
                FirstName = "Nativat",
                Offset = 300,
            };
            _entitySqlCommand.Setup(s => s.ListSqlCommand).Returns(_listSqlCommand);
            _dapperService.Setup(d => d.QueryAsync<TestEntity>(_listSqlCommand, sqlparam)).ReturnsAsync(PersonList);

            var actual = await _repository.ListAsync<TestEntity>(sqlparam);

            Assert.Equal(PersonList, actual);
            _dapperService.VerifyAll();
        }

        [Fact]
        public async Task ListAsyncPropagatesException()
        {
            var sqlparam = new ListParamEntity { FirstName = "Nativat", Offset = 300, };
            _entitySqlCommand.Setup(s => s.ListSqlCommand).Returns(_listSqlCommand);
            var expectedException = new Exception(_testExceptionMessage);
            _dapperService.Setup(d => d.QueryAsync<TestEntity>(_listSqlCommand, sqlparam)).ThrowsAsync(expectedException);

            async Task Act() => await _repository.ListAsync<TestEntity>(sqlparam);

            var actualException = await Assert.ThrowsAsync<Exception>(Act);
            Assert.Equal(expectedException, actualException);
            _dapperService.VerifyAll();
        }

        [Fact]
        public async Task CreateAsyncReturnsOne()
        {
            var expected = 1;
            _entitySqlCommand.Setup(s => s.CreateSqlCommand).Returns(_createSqlCommand);
            _dapperService.Setup(d => d.ExecuteAsync(_createSqlCommand, Person1)).ReturnsAsync(expected);

            var actual = await _repository.CreateAsync(Person1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CreateAsyncPropagatesException()
        {
            var expected = new Exception(_testExceptionMessage);
            _entitySqlCommand.Setup(s => s.CreateSqlCommand).Returns(_createSqlCommand);
            _dapperService.Setup(d => d.ExecuteAsync(_createSqlCommand, Person1)).ThrowsAsync(expected);

            async Task Act() => await _repository.CreateAsync(Person1);
            var actual = await Assert.ThrowsAsync<Exception>(Act);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAsyncReturnsOne()
        {
            var expected = 1;
            _entitySqlCommand.Setup(s => s.UpdateSqlCommand).Returns(_updateSqlCommand);
            _dapperService.Setup(d => d.ExecuteAsync(_updateSqlCommand, Person1)).ReturnsAsync(expected);

            var actual = await _repository.UpdateAsync(Person1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAsyncPropagatesException()
        {
            var expected = new Exception(_testExceptionMessage);
            _entitySqlCommand.Setup(s => s.UpdateSqlCommand).Returns(_updateSqlCommand);
            _dapperService.Setup(d => d.ExecuteAsync(_updateSqlCommand, Person1)).ThrowsAsync(expected);

            async Task Act() => await _repository.UpdateAsync(Person1);
            var actual = await Assert.ThrowsAsync<Exception>(Act);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteAsyncReturnsOne()
        {
            var expected = 1;
            _entitySqlCommand.Setup(s => s.DeleteSqlCommand).Returns(_deleteSqlCommand);
            _dapperService.Setup(d => d.ExecuteAsync(_deleteSqlCommand, EntityIdParam)).ReturnsAsync(expected);

            var actual = await _repository.DeleteAsync(EntityIdParam);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteAsyncPropagatesException()
        {
            var expected = new Exception(_testExceptionMessage);
            _entitySqlCommand.Setup(s => s.DeleteSqlCommand).Returns(_deleteSqlCommand);
            _dapperService.Setup(d => d.ExecuteAsync(_deleteSqlCommand, EntityIdParam)).ThrowsAsync(expected);

            async Task Act() => await _repository.DeleteAsync(EntityIdParam);
            var actual = await Assert.ThrowsAsync<Exception>(Act);

            Assert.Equal(expected, actual);
        }
    }
}