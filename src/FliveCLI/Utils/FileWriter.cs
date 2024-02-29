using FliveCLI.TableEntities;

namespace FliveCLI.Utils
{
    public static class FileWriter
    {
        private static string GetFolderPath(string folderName)
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = "Gol";
            return Path.Combine(appDataFolder, appFolder, folderName);
        }

        internal static void DeleteFile(string fileName, string folderName)
        {
            var fullPath = GetFolderPath(folderName);
            fullPath = Path.Combine(fullPath, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        internal static void WriteDBScript(StringContentList stringContent, string fileName, string folderName)
        {
            var fullPath = GetFolderPath(folderName);
            Directory.CreateDirectory(fullPath);

            fullPath = Path.Combine(fullPath, fileName);
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

        internal static void WriteEntitySql(TableEntity tableEntity, string folderName, string namespaceName = "")
        {
            var fileName = tableEntity.ClassName + "EntitySql.cs";
            DeleteFile(fileName, folderName);

            var fullPath = GetFolderPath(folderName);
            Directory.CreateDirectory(fullPath);

            fullPath = Path.Combine(fullPath, fileName);
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
    }
}