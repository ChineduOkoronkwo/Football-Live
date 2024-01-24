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
            var entityId = new EntityId { Id = 1 };
            _entitySqlCommand.Setup(s => s.GetSqlCommand).Returns(_getSqlCommand);
            _dapperService.Setup(d => d.QuerySingleAsync<TestEntity>(_getSqlCommand, entityId)).ReturnsAsync(Person1);
            var actual = await _repository.GetAsync<TestEntity>(entityId);
            Assert.Equal(Person1, actual);
            _dapperService.VerifyAll();
        }

        [Fact]
        public async Task GetAsyncPropagatesException()
        {
            var entityId = new EntityId { Id = 1 };
            var expectedException = new Exception(_testExceptionMessage);
            _entitySqlCommand.Setup(s => s.GetSqlCommand).Returns(_getSqlCommand);
            _dapperService.Setup(d => d.QuerySingleAsync<TestEntity>(_getSqlCommand, entityId)).ThrowsAsync(expectedException);

            async Task Act() => await _repository.GetAsync<TestEntity>(entityId);
            var actualException = await Assert.ThrowsAsync<Exception>(Act);
            Assert.Equal(expectedException, actualException);
            _dapperService.VerifyAll();
        }
    }
}