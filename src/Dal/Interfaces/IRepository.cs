namespace Dal.Interfaces
{
    public interface IRepository
    {
        Task<T> GetAsync<T>(object? param);
        Task<IEnumerable<T>> ListAsync<T>(object? param);
        Task<int> CreateAsync(object? param);
        Task<int> DeleteAsync(object? param);
        Task<int> UpdateAsync(object? param);
    }
}