using System.Data;
using Dapper;
using DbService.Interfaces;

namespace DbService.Services
{
    public class QueryService(IDbConnection dbConnection) : IQueryService
    {
        public async Task<int> ExecuteAsync(string sqlCommand, object? param)
        {
            return await dbConnection.ExecuteAsync(sqlCommand, param);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? param)
        {
            return await dbConnection.QueryAsync<T>(sqlCommand, param);
        }

        public async Task<T> QuerySingleAsync<T>(string sqlCommand, object? param)
        {
            return await dbConnection.QuerySingleAsync<T>(sqlCommand, param);
        }
    }
}