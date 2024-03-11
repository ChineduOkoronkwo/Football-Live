using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class RepoWriter
    {
        internal static string Foldername => "Repos";
        private static string FileNamePrefix => "Repo.cs";

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

            writer.WriteLine($"public class {tableEntity.ClassName}Repo(IDapperService dapperService, IEntitySqlCommand entitySqlCommand) : IGenericRepository");
            writer.WriteLine("{");

            // GetAsync
            var tEntity = tableEntity.TEntityName;
            var tParam = tableEntity.TGetParamName;
            writer.WriteLine($"{tab4}public async Task<{tEntity}> GetAsync<{tEntity}, {tParam}>({tParam} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.QueryAsync<{tEntity}>(entitySqlCommand.ListSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            // ListAsync
            tParam = tableEntity.TListParamName;
            writer.WriteLine($"{tab4}public async Task<IEnumerable<{tEntity}>> ListAsync<{tEntity}, {tParam}>(tParam? param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.QuerySingleAsync<{tEntity}>(entitySqlCommand.GetSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            // CreateAsync
            writer.WriteLine($"{tab4}public async Task<int> CreateAsync<{tEntity}>({tEntity} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.ExecuteAsync<{tEntity}>(entitySqlCommand.CreateSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            // UpdateAsync
            writer.WriteLine($"{tab4}public async Task<int> UpdateAsync<{tEntity}>({tEntity} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.ExecuteAsync<{tEntity}>(entitySqlCommand.UpdateSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            // DeleteAsync
            writer.WriteLine($"{tab4}public async Task<int> DeleteAsync<{tEntity}>({tEntity} param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dapperService.ExecuteAsync<{tEntity}>(entitySqlCommand.DeleteSqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            writer.WriteLine("}");
        }
    }
}