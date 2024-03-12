using FliveCLI.Handlers;
using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class FileWriter
    {
        private static string tab4 = "    ";
        private static string tab8 = "        ";
        internal static void CreateRepo(TableEntity[] tableEntities, HashSet<string> exclude, string projectName)
        {
            var deleted = DeleteAndCreateDirectories(true);
            if (deleted)
            {
                // create repo project
                DotnetHandler.CreateProject(projectName);

                // Create Directories for organizing project classes
                CreateDirectories(projectName);

                // write data files
                var visited = new HashSet<TableEntity>();
                var createdDtos = new HashSet<string>();
                foreach (var entity in tableEntities)
                {
                    if (!(exclude.Contains(entity.ClassName) || exclude.Contains(entity.FullName)))
                    {
                        CreateRepo(entity, visited, createdDtos, projectName);
                    }
                }

                // Write interfaces and services
                WriteDalFiles(projectName);
            }
            else
            {
                Console.WriteLine("Operation was cancelled");
            }
        }

        private static void CreateRepo(TableEntity tableEntity, HashSet<TableEntity> visited, HashSet<string> createdDtos, string projectName)
        {
            if (visited.Contains(tableEntity))
            {
                return;
            }

            visited.Add(tableEntity);

            foreach (var entity in tableEntity.RefEntities)
            {
                CreateRepo(entity, visited, createdDtos, projectName);
            }

            // Write repo files
            WriteEntityRepoFiles(tableEntity, createdDtos, projectName);
        }

        private static void WriteDalFiles(string projectName)
        {
            // write interfaces
            InterfaceWriter.Write(projectName, tab4);

            // write dapper service
            ServiceWriter.Write(projectName, tab4, tab8);
        }

        private static void WriteEntityRepoFiles(TableEntity tableEntity, HashSet<string> createdDtos, string projectName)
        {
            var append = true;

            // write db script
            DbScriptWriter.Write(tableEntity, append, tab4);

            // write entity sql class
            EntitySqlWriter.Write(tableEntity, append, tab4, tab8, projectName);

            // write db dto class
            DbDtoWrite.Write(tableEntity, createdDtos, projectName);

            // write repo class
            RepoWriter.Write(tableEntity, append, tab4, tab8, projectName);
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

            return true;
        }

        private static void CreateDirectories(string projectName)
        {
            Directory.CreateDirectory(FileUtil.GetPath(projectName, DbDtoWrite.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(projectName, EntitySqlWriter.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(DbScriptWriter.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(projectName, RepoWriter.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(projectName, InterfaceWriter.Foldername));
            Directory.CreateDirectory(FileUtil.GetPath(projectName, ServiceWriter.Foldername));
        }
    }
}