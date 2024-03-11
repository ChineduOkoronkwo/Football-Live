namespace Dal.Interfaces
{
    public interface IGenericRepository
    {
        Task<TEntity> GetAsync<TEntity, TParam>(TParam param);
        Task<IEnumerable<TEntity>> ListAsync<TEntity, TParam>(TParam? param);
        Task<int> CreateAsync<TParam>(TParam param);
        Task<int> DeleteAsync<TParam>(TParam param);
        Task<int> UpdateAsync<TParam>(TParam param);
    }
}