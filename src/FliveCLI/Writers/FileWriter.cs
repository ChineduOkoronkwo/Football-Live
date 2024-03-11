using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class FileWriter
    {
        internal static void CreateRepo(TableEntity[] tableEntities)
        {
            var deleted = DeleteAndCreateDirectories(true);
            if (deleted)
            {
                var visited = new HashSet<TableEntity>();
                var createdDtos = new HashSet<string>();
                foreach (var entity in tableEntities)
                {
                    CreateRepo(entity, visited, createdDtos);
                }
            }
            else
            {
                Console.WriteLine("Operation was cancelled");
            }
        }

        private static void CreateRepo(TableEntity tableEntity, HashSet<TableEntity> visited, HashSet<string> createdDtos)
        {
            if (visited.Contains(tableEntity))
            {
                return;
            }

            visited.Add(tableEntity);

            foreach (var entity in tableEntity.RefEntities)
            {
                CreateRepo(entity, visited, createdDtos);
            }

            // Write repo files
            WriteEntityRepoFiles(tableEntity, createdDtos);
        }

        private static void WriteEntityRepoFiles(TableEntity tableEntity, HashSet<string> createdDtos)
        {
            var tab4 = "    ";
            var tab8 = "        ";
            var append = true;

            // write db script
            DbScriptWriter.Write(tableEntity, append, tab4);

            // write entity sql class
            EntitySqlWriter.Write(tableEntity, append, tab4, tab8);

            // write db dto class
            DbDtoWrite.Write(tableEntity, createdDtos);

            // write repo class
            RepoWriter.Write(tableEntity, append, tab4, tab8);
        }

        private static bool DeleteAndCreateDirectories(bool confirmed = false)
        {
            var path = FileUtil.GetPath();
            if (!confirmed)
            {
                Console.WriteLine($"Warning! All existing contents of {path} will be deleted!\nEnter Y to proceed, N to Cancel");
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

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(FileUtil.GetPath(DbDtoWrite.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(EntitySqlWriter.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(DbScriptWriter.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(RepoWriter.Foldername));
            return true;
        }
    }
}