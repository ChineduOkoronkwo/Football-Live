using FliveCLI.TableEntities;

namespace FliveCLI.Writers
{
    public static class FileWriter
    {
        internal static string GetPath(string folderName = "", string fileName = "")
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = "Gol";
            return Path.Combine(appDataFolder, appFolder, folderName, fileName);
        }

        internal static void WriteEntityRepoFiles(TableEntity tableEntity, HashSet<string> createdDtos)
        {
            var tab4 = "    ";
            var tab8 = "        ";
            var append = true;

            // write db script
            var filePath = GetPath(DbScriptWriter.Foldername, DbScriptWriter.FileNamePrefix);
            DbScriptWriter.Write(tableEntity, filePath, append, tab4);

            // write entity sql class
            filePath = GetPath(EntitySqlWriter.Foldername, tableEntity.ClassName + EntitySqlWriter.FileNamePrefix);
            EntitySqlWriter.Write(tableEntity, filePath, append, tab4, tab8);

            // write db dto class
            DbDtoWrite.Write(tableEntity, createdDtos);
        }

        internal static bool DeleteAndCreateDirectories(bool confirmed = false)
        {
            if (!confirmed)
            {
                Console.WriteLine($"Warning! All existing contents of {GetPath()} will be deleted!\nEnter Y to proceed, N to Cancel");
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

            var path = GetPath();
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(GetPath(DbDtoWrite.Foldername));
            Directory.CreateDirectory(GetPath(EntitySqlWriter.Foldername));
            Directory.CreateDirectory(GetPath(DbScriptWriter.Foldername));
            return true;
        }
    }
}