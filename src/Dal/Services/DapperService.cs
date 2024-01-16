using System.Data;
using Dal.Interfaces;
using Dapper;

namespace Dal.Services
{
    public class DapperService(IDbConnection dbConnection) : IDapperService
    {
        public async Task<int> ExecuteAsync(string sqlCommand, object? param)
        {
            return await dbConnection.ExecuteAsync(sqlCommand, param);
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sqlCommand, object? param)
        {
            return await dbConnection.ExecuteScalarAsync<T>(sqlCommand, param);
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