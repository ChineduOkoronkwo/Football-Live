using System.Reflection;

namespace DbService.Utilities
{
    public static class SqlGenerator
    {
        public static string GenerateGetSqlCommand(Type TEntityType, Type TEntityIdType)
        {
            var tEntityPropertyNames = GetPropertiesNames(GetTEntityPropertyInfos(TEntityType));
            var tIdEntityPropertyNames = GetPropertiesNames(GetTEntityPropertyInfos(TEntityIdType));
            for (int index = 0; index < tIdEntityPropertyNames.Count; index++)
            {
                tIdEntityPropertyNames[index] = $"{tIdEntityPropertyNames[index]} = @{tIdEntityPropertyNames[index]}";
            }

            return $"SELECT {string.Join(", ", tEntityPropertyNames)} FROM {TEntityType.Name} "
                + $"WHERE {string.Join(" AND ", tIdEntityPropertyNames)};";
        }

        private static PropertyInfo[] GetTEntityPropertyInfos(Type type)
            => type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private static List<string> GetPropertiesNames(PropertyInfo[] properties)
        {
            var result = new List<string>();
            foreach (var property in properties)
            {
                result.Add(property.Name);
            }

            return result;
        }
    }
}