namespace Dal.Interfaces
{
    public interface IRepository
    {
        Task<T> Get<T>(object? param);
        Task<IEnumerable<T>> List<T>(object? param);
        Task<int> Create(object? param);
        Task<int> Delete(object? param);
        Task<int> Update(object? param);
    }
}