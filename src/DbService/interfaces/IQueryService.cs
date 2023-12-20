namespace DbService.interfaces
{
    public interface IQueryService
    {
        Task<int> ExecuteAsync(string sqlCommand, object? param);
        Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? param);
        Task<T> QuerySingleAsync<T>(string sqlCommand, object? param);
    }
}