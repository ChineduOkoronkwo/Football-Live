using Dal.Interfaces;

namespace Dal.Services
{
    public class Repository(IDapperService dapperService, IEntitySqlCommand entitySqlCommand) : IRepository
    {
        public async Task<T> Get<T>(object? param)
        {
            return await dapperService.QuerySingleAsync<T>(entitySqlCommand.GetSqlCommand, param);
        }

        public async Task<IEnumerable<T>> List<T>(object? param)
        {
            return await dapperService.QueryAsync<T>(entitySqlCommand.ListSqlCommand, param);
        }

        public async Task<int> Create(object? param)
        {
            return await dapperService.ExecuteAsync(entitySqlCommand.CreateSqlCommand, param);
        }

        public async Task<int> Update(object? param)
        {
            return await dapperService.ExecuteAsync(entitySqlCommand.UpdateSqlCommand, param);
        }

        public async Task<int> Delete(object? param)
        {
            return await dapperService.ExecuteAsync(entitySqlCommand.DeleteSqlCommand, param);
        }
    }
}