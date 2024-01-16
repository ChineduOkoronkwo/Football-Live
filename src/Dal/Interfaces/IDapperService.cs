using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IDapperService
    {
        Task<int> ExecuteAsync(string sqlCommand, object? param);
        Task<T?> ExecuteScalarAsync<T>(string sqlCommand, object? param);
        Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? param);
        Task<T> QuerySingleAsync<T>(string sqlCommand, object? param);
    }
}