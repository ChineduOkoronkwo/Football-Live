using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class ServiceWriter
    {
        internal static string Foldername => "Services";

        internal static void Write(string projectName, string tab4, string tab8, string namespaceName = "Dal.Services")
        {
            WriteDapperService(projectName, tab4, tab8, namespaceName);
        }

        private static void WriteDapperService(string projectName, string tab4, string tab8, string namespaceName)
        {
            var fullPath = FileUtil.GetPath(projectName, Foldername, "DapperService.cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using Dal.Interfaces;");
            writer.WriteLine("using Dapper;");
            writer.WriteLine();

            writer.WriteLine($"namespace {namespaceName};");
            writer.WriteLine();
            writer.WriteLine("public class DapperService(IDbConnection dbConnection) : IDapperService");
            writer.WriteLine("{");

            // ExecuteAsync
            writer.WriteLine($"{tab4}public async Task<int> ExecuteAsync(string sqlCommand, object? param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dbConnection.ExecuteAsync(sqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // ExecuteScalarAsync
            writer.WriteLine($"{tab4}public async Task<T?> ExecuteScalarAsync<T>(string sqlCommand, object? param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dbConnection.ExecuteScalarAsync<T>(sqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // QueryAsync
            writer.WriteLine($"{tab4}public async Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dbConnection.QueryAsync<T>(sqlCommand, param);");
            writer.WriteLine($"{tab4}}}");
            writer.WriteLine();

            // QuerySingleAsync
            writer.WriteLine($"{tab4}public async Task<T> QuerySingleAsync<T>(string sqlCommand, object? param)");
            writer.WriteLine($"{tab4}{{");
            writer.WriteLine($"{tab8}return await dbConnection.QuerySingleAsync<T>(sqlCommand, param);");
            writer.WriteLine($"{tab4}}}");

            writer.WriteLine("}");
        }
    }
}