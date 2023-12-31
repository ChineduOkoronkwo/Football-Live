using System.Reflection;
using DbService.Interfaces;

namespace DbService.Services
{
    public class Repository<TEntity, TIdEntity> : IRepository<TEntity>
        where TEntity : TIdEntity
    {
        public virtual string GetSqlCommand
        {
            get
            {
                var tEntityPropertyNames = GetPropertiesNames(GetTEntityPropertyInfos);
                var tIdEntityPropertyNames = GetPropertiesNames(GetTIdEntityPropertyInfos);
                for (int index = 0; index < tIdEntityPropertyNames.Count; index++)
                {
                    tIdEntityPropertyNames[index] = $"{tIdEntityPropertyNames[index]} = @{tIdEntityPropertyNames[index]}";
                }

                return $"SELECT {string.Join(", ", tEntityPropertyNames)} FROM {typeof(TEntity).Name} "
                    + $"WHERE {string.Join(" AND ", tIdEntityPropertyNames)};";
            }
        }

        public Task<TEntity> GetAsync<TGetFilter>(TGetFilter? param, bool convertToSqlParm)
        {
            throw new NotImplementedException();
        }

        private List<string> GetPropertiesNames(PropertyInfo[] properties)
        {
            var result = new List<string>();
            foreach (var property in properties)
            {
                result.Add(property.Name);
            }

            return result;
        }

        private PropertyInfo[] GetTEntityPropertyInfos
            => typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private PropertyInfo[] GetTIdEntityPropertyInfos
            => typeof(TIdEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
}