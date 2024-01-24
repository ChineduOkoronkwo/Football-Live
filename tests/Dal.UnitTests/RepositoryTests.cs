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
            var sqlparam = new EntityId { Id = 1 };
            _entitySqlCommand.Setup(s => s.GetSqlCommand).Returns(_getSqlCommand);
            _dapperService.Setup(d => d.QuerySingleAsync<TestEntity>(_getSqlCommand, sqlparam)).ReturnsAsync(Person1);

            var actual = await _repository.GetAsync<TestEntity>(sqlparam);

            Assert.Equal(Person1, actual);
            _dapperService.VerifyAll();
        }

        [Fact]
        public async Task GetAsyncPropagatesException()
        {
            var sqlparam = new EntityId { Id = 1 };
            var expectedException = new Exception(_testExceptionMessage);
            _entitySqlCommand.Setup(s => s.GetSqlCommand).Returns(_getSqlCommand);
            _dapperService.Setup(d => d.QuerySingleAsync<TestEntity>(_getSqlCommand, sqlparam)).ThrowsAsync(expectedException);

            async Task Act() => await _repository.GetAsync<TestEntity>(sqlparam);

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
    }
}