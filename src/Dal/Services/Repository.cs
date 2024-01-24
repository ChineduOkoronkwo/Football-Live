using Dal.Interfaces;

namespace Dal.Services
{
    public class Repository(IDapperService dapperService, IEntitySqlCommand entitySqlCommand) : IRepository
    {
        public async Task<T> GetAsync<T>(object? param)
        {
            return await dapperService.QuerySingleAsync<T>(entitySqlCommand.GetSqlCommand, param);
        }

        public async Task<IEnumerable<T>> ListAsync<T>(object? param)
        {
            return await dapperService.QueryAsync<T>(entitySqlCommand.ListSqlCommand, param);
        }

        public async Task<int> CreateAsync(object? param)
        {
            return await dapperService.ExecuteAsync(entitySqlCommand.CreateSqlCommand, param);
        }

        public async Task<int> UpdateAsync(object? param)
        {
            return await dapperService.ExecuteAsync(entitySqlCommand.UpdateSqlCommand, param);
        }

        public async Task<int> DeleteAsync(object? param)
        {
            return await dapperService.ExecuteAsync(entitySqlCommand.DeleteSqlCommand, param);
        }
    }
}