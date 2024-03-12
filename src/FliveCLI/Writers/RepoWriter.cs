using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class RepoWriter
    {
        internal static string Foldername => "Repos";
        private static string FileNamePrefix => "Repo.cs";

        internal static void Write(TableEntity tableEntity, bool append, string tab4, string tab8, string projectName, string namespaceName = "Dal.Repos")
        {
            var filePath = FileUtil.GetPath(projectName, Foldername, tableEntity.ClassName + FileNamePrefix);
            using StreamWriter writer = new StreamWriter(filePath, append);
            writer.WriteLine("using Dal.Dtos;");
            writer.WriteLine("using Dal.Interfaces;");
            writer.WriteLine();

            if (!string.IsNullOrWhiteSpace(namespaceName))
            {
                writer.WriteLine($"namespace {namespaceName};");
                writer.WriteLine();
            }

            writer.WriteLine($"public class {tableEntity.ClassName}Repo(IDapperService dapperService, IEntitySqlCommand entitySqlCommand) : IGenericRepository");
            writer.WriteLine("{");

            // GetAsync
            var tEntity = tableEntity.TEntityName;
            var tParam = tableEntity.TGetParamEntityName;
            writer.WriteLine($"{tab4}public async Task<{tEntity}> GetAsync({tParam} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.QuerySingleAsync<{tEntity}>(entitySqlCommand.ListSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // ListAsync
            tParam = tableEntity.TListParamEntityName;
            writer.WriteLine($"{tab4}public async Task<IEnumerable<{tEntity}>> ListAsync({tParam}? param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.QueryAsync<{tEntity}>(entitySqlCommand.GetSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // CreateAsync
            writer.WriteLine($"{tab4}public async Task<int> CreateAsync({tEntity} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.ExecuteAsync(entitySqlCommand.CreateSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // UpdateAsync
            writer.WriteLine($"{tab4}public async Task<int> UpdateAsync({tEntity} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.ExecuteAsync(entitySqlCommand.UpdateSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // DeleteAsync
            writer.WriteLine($"{tab4}public async Task<int> DeleteAsync({tEntity} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.ExecuteAsync(entitySqlCommand.DeleteSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            writer.WriteLine("}");
        }
    }
}