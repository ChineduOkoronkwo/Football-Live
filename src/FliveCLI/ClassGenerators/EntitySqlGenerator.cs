using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.ClassGenerators
{
    public static class EntitySqlGenerator
    {
        public static StringContentList GenerateEntitySql(TableEntity tableEntity, string className, string namespaceName = "")
        {
            var tab4 = "    ";
            var tab8 = "        ";
            var sqlList = new StringContentList { "using Dal.Interfaces;", "" };
            if (!string.IsNullOrWhiteSpace(namespaceName))
            {
                sqlList.Add($"namespace {namespaceName};");
            }

            sqlList.Add($"public class {className}EntitySql : IEntitySqlCommand");
            sqlList.Add("{");

            // GetSqlCommand
            sqlList.Add($"{tab4}public string GetSqlCommand");
            sqlList.Add($"{tab8}=> \"{tableEntity.GenerateGetSql()}\"");

            // ListSqlCommand
            sqlList.Add($"{tab4}public string ListSqlCommand");
            sqlList.Add($"{tab8}=> \"{tableEntity.GenerateListSql()}\"");

            // CreateSqlCommand
            sqlList.Add($"{tab4}public string CreateSqlCommand");
            sqlList.Add($"{tab8}=> \"{tableEntity.GenerateCreateSql()}\"");

            // DeleteSqlCommand
            sqlList.Add($"{tab4}public string DeleteSqlCommand");
            sqlList.Add($"{tab8}=> \"{tableEntity.GenerateDeleteSql()}\"");

            // UpdateSqlCommand
            sqlList.Add($"{tab4}public string UpdateSqlCommand");
            sqlList.Add($"{tab8}=> \"{tableEntity.GenerateUpdateSql()}\"");

            sqlList.Add("}");
            return sqlList;
        }
    }
}