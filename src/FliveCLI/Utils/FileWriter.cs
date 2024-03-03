using System.Text;
using FliveCLI.EntityColumns;
using FliveCLI.TableEntities;

namespace FliveCLI.Utils
{
    public static class FileWriter
    {
        private static string DbDtoFolderName => "DbDtos";
        private static string EntitySqlFolderName => "EntitySqls";
        private static string DbScriptFoldername => "DbScripts";

        private static string GetFolderPath(string folderName = "", string fileName = "")
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = "Gol";
            return Path.Combine(appDataFolder, appFolder, folderName, fileName);
        }

        internal static bool DeleteAndCreateDirectories(bool confirmed = false)
        {
            if (!confirmed)
            {
                Console.WriteLine($"Warning! All existing contents of {GetFolderPath()} will be deleted!\nEnter Y to proceed, N to Cancel");
                var response = char.ToUpper((char)Console.Read());
                confirmed = response switch
                {
                    'Y' => true,
                    'N' => false,
                    _ => throw new ArgumentException("Invalid response! Expected Y or N, got {response}."),
                };
            }

            if (!confirmed)
            {
                return false;
            }

            var path = GetFolderPath();
            if (Directory.Exists(path))
            {
                Directory.Delete(GetFolderPath(), true);
            }

            Directory.CreateDirectory(GetFolderPath(DbDtoFolderName));
            Directory.CreateDirectory(GetFolderPath(EntitySqlFolderName));
            Directory.CreateDirectory(GetFolderPath(DbScriptFoldername));
            return true;
        }

        internal static void WriteDBScript(StringContentList stringContent)
        {
            var fullPath = GetFolderPath(DbScriptFoldername, "DbScripts.sql");
            bool append = true;
            var tab = "    ";

            using StreamWriter writer = new StreamWriter(fullPath, append);
            var lastLine = stringContent.Count - 1;
            for (int index = 0; index <= lastLine; index++)
            {
                if (index > 0 && index < lastLine)
                {
                    writer.Write(tab);
                }

                writer.WriteLine(stringContent[index]);
            }

            writer.WriteLine();
        }

        internal static void WriteEntitySql(TableEntity tableEntity, string namespaceName = "")
        {
            var fileName = tableEntity.ClassName + "EntitySql.cs";
            var fullPath = GetFolderPath(EntitySqlFolderName, fileName);
            bool append = true;
            var tab4 = "    ";
            var tab8 = "        ";

            using StreamWriter writer = new StreamWriter(fullPath, append);
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

        internal static void WriteDto(TableEntity tableEntity,  string namespaceName = "")
        {
            var usingStatements = new HashSet<string>();
            var classFields = new StringBuilder();
            ProcessFields(tableEntity.EntityColumns, usingStatements, classFields);

            var fullPath = GetFolderPath(DbDtoFolderName, tableEntity.ClassName + "Dto.cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            foreach (var item in usingStatements)
            {
                writer.WriteLine($"using {item};");
            }

            if (usingStatements.Count > 0)
            {
                writer.WriteLine();
            }

            if (!string.IsNullOrWhiteSpace(namespaceName))
            {
                writer.WriteLine($"namespace {namespaceName};");
                writer.WriteLine();
            }

            writer.WriteLine($"public class {tableEntity.ClassName}Dto");
            writer.WriteLine("{");
            writer.Write(classFields.ToString());
            writer.WriteLine("}");
        }

        private static void ProcessFields(List<BaseEntityColumn> entityColumns, HashSet<string> usingStatements, StringBuilder classFields)
        {
            foreach (var col in entityColumns)
            {
                var lastDotIndex = col.FieldType.LastIndexOf('.');
                if (lastDotIndex > 0)
                {
                    usingStatements.Add(col.FieldType[..lastDotIndex]);
                }

                var nullable = col.IsNullable ? "?" : "";
                var fieldType = DotnetTypeMapping.TypeMapper[col.FieldType];
                classFields.AppendLine($"    public {fieldType}{nullable} {col.FieldName} {{ get; set; }}");
            }
        }
    }
}