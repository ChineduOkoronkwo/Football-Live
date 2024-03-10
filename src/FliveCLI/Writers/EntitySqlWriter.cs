using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class EntitySqlWriter
    {
        internal static string Foldername => "EntitySqls";
        private static string FileNamePrefix => "EntitySql.cs";
        internal static void Write(TableEntity tableEntity, bool append, string tab4, string tab8, string namespaceName = "")
        {
            var filePath = FileUtil.GetPath(Foldername, tableEntity.ClassName + FileNamePrefix);
            using StreamWriter writer = new StreamWriter(filePath, append);
            writer.WriteLine("using Dal.Interfaces;");
            writer.WriteLine();

            if (!string.IsNullOrWhiteSpace(namespaceName))
            {
                writer.WriteLine($"namespace {namespaceName};");
                writer.WriteLine();
            }

            writer.WriteLine($"public class {tableEntity.ClassName}EntitySql : IEntitySqlCommand");
            writer.WriteLine("{");

            // GetSqlCommand
            writer.WriteLine($"{tab4}public string GetSqlCommand");
            writer.WriteLine($"{tab8}=> \"{tableEntity.GenerateGetSql()}\"");

            // ListSqlCommand
            writer.WriteLine($"{tab4}public string ListSqlCommand");
            writer.WriteLine($"{tab8}=> \"{tableEntity.GenerateListSql()}\"");

            // CreateSqlCommand
            writer.WriteLine($"{tab4}public string CreateSqlCommand");
            writer.WriteLine($"{tab8}=> \"{tableEntity.GenerateCreateSql()}\"");

            // DeleteSqlCommand
            writer.WriteLine($"{tab4}public string DeleteSqlCommand");
            writer.WriteLine($"{tab8}=> \"{tableEntity.GenerateDeleteSql()}\"");

            // UpdateSqlCommand
            writer.WriteLine($"{tab4}public string UpdateSqlCommand");
            writer.WriteLine($"{tab8}=> \"{tableEntity.GenerateUpdateSql()}\"");

            writer.WriteLine("}");
        }
    }
}