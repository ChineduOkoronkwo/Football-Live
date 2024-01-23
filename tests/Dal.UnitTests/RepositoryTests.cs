using Dal.Interfaces;
using Dal.Services;
using Dal.UnitTests.Utils;
using Moq;

namespace Dal.UnitTests
{
    public class RepositoryTests : TestUtil
    {
        private Mock<IDapperService> _dapperService;
        private Mock<IEntitySqlCommand> _entitySqlCommand;
        private IRepository _repository;

        public RepositoryTests()
        {
            _dapperService = new Mock<IDapperService>();
            _entitySqlCommand = new Mock<IEntitySqlCommand>();
            _repository = new Repository(_dapperService.Object, _entitySqlCommand.Object);
            SetupSquelCommand();
        }

        private void SetupSquelCommand()
        {
            _entitySqlCommand
                .SetupProperty(x => x.GetSqlCommand, _getSqlCommand)
                .SetupProperty(x => x.ListSqlCommand, _listSqlCommand)
                .SetupProperty(x => x.CreateSqlCommand, _createSqlCommand)
                .SetupProperty(x => x.UpdateSqlCommand, _updateSqlCommand)
                .SetupProperty(x => x.DeleteSqlCommand, _deleteSqlCommand);
        }
    }
}