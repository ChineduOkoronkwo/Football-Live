using DbService.Interfaces;
using DbService.Utilities;

namespace DbService.Services
{
    public class Repository<TEntity, TIdEntity> : IRepository<TEntity>
        where TEntity : TIdEntity
    {
        private readonly IQueryService _queryService;
        public Repository(IQueryService queryService)
        {
            _queryService = queryService;
        }

        public virtual string GetSqlCommand => SqlGenerator.GenerateGetSqlCommand(typeof(TEntity), typeof(TIdEntity));

        public Task<TEntity> GetAsync<TGetFilter>(TGetFilter param, bool convertToSqlParm = false)
        {
            object? sqlParm = convertToSqlParm ? null : param;
            return _queryService.QuerySingleAsync<TEntity>(GetSqlCommand, sqlParm);
        }
    }
}