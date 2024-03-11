using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class InterfaceWriter
    {
        internal static string Foldername => "Interfaces";
        public static void Write(string tab4, string namespaceName = "Dal.Interfaces")
        {
            WriteIDapperService(tab4, namespaceName);
            WriteIEntitySqlCommand(tab4, namespaceName);
            WriteIGenericRepository(tab4, namespaceName);
        }

        private static void WriteIDapperService(string tab4, string namespaceName)
        {
            var fullPath = FileUtil.GetPath(Foldername, "IDapperService.cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            writer.WriteLine($"namespace {namespaceName};");
            writer.WriteLine();
            writer.WriteLine("public interface IDapperService");
            writer.WriteLine("{");
            writer.WriteLine($"{tab4}Task<int> ExecuteAsync(string sqlCommand, object? param);");
            writer.WriteLine($"{tab4}Task<T?> ExecuteScalarAsync<T>(string sqlCommand, object? param);");
            writer.WriteLine($"{tab4}Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? param);");
            writer.WriteLine($"{tab4}Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? param);");
            writer.WriteLine("}");
        }

        private static void WriteIEntitySqlCommand(string tab4, string namespaceName)
        {
            var fullPath = FileUtil.GetPath(Foldername, "IEntitySqlCommand.cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            writer.WriteLine($"namespace {namespaceName};");
            writer.WriteLine();
            writer.WriteLine("public interface IEntitySqlCommand");
            writer.WriteLine("{");
            writer.WriteLine($"{tab4}string GetSqlCommand {{ get; }}");
            writer.WriteLine($"{tab4}string ListSqlCommand {{ get; }}");
            writer.WriteLine($"{tab4}string CreateSqlCommand {{ get; }}");
            writer.WriteLine($"{tab4}string DeleteSqlCommand {{ get; }}");
            writer.WriteLine($"{tab4}string UpdateSqlCommand {{ get; }}");
            writer.WriteLine("}");
        }

        private static void WriteIGenericRepository(string tab4, string namespaceName)
        {
            var fullPath = FileUtil.GetPath(Foldername, "IGenericRepository.cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            writer.WriteLine($"namespace {namespaceName};");
            writer.WriteLine();
            writer.WriteLine("public interface IGenericRepository");
            writer.WriteLine("{");
            writer.WriteLine($"{tab4}Task<TEntity> GetAsync<TEntity, TParam>(TParam param);");
            writer.WriteLine($"{tab4}Task<IEnumerable<TEntity>> ListAsync<TEntity, TParam>(TParam? param);");
            writer.WriteLine($"{tab4}Task<int> CreateAsync<TParam>(TParam param);");
            writer.WriteLine($"{tab4}Task<int> DeleteAsync<TParam>(TParam param);");
            writer.WriteLine($"{tab4}Task<int> UpdateAsync<TParam>(TParam param);");
            writer.WriteLine("}");
        }
    }
}