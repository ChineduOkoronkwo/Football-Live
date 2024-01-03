namespace DbService.Interfaces
{
    public interface IRepository<TEntity>
    {
        string GetSqlCommand { get; }
        Task<TEntity> GetAsync<TGetFilter>(TGetFilter param, bool convertToSqlParm = false);
    }
}