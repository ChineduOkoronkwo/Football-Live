using System.CommandLine;
using System.Reflection;
using System.Text;
using FliveCLI;
using FliveCLI.EntityColumns;
using FliveCLI.TableEntities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace flive;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var entityDirOption = new Option<string>(
            name: "--entity-dir",
            description: "Path to the directory that contains the .cs entity files."
        );

        var rootCommand = new RootCommand("A CLI that generates repos and sql commands for a given entity");
        rootCommand.AddOption(entityDirOption);
        rootCommand.SetHandler(GenRepo, entityDirOption);
        return await rootCommand.InvokeAsync(args);
    }

    internal static void GenRepo(string dirPath)
    {
        var entities = CreateEntities(dirPath);
        Console.WriteLine(GetCreateTableSql(entities));
    }

    internal static TableEntity[] CreateEntities(string dirPath)
    {
        var files = Directory.GetFiles(dirPath, "*.cs");

        // Create a compilation
        var compilation = CSharpCompilation.Create("DynamicAssembly")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

        // Add the .cs files to the compilation
        foreach (var file in files)
        {
            var text = File.ReadAllText(file);
            var syntaxTree = CSharpSyntaxTree.ParseText(text);
            compilation = compilation.AddSyntaxTrees(syntaxTree);
        }

        // Compile the code
        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);
        if (!result.Success)
        {
            var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

            foreach (var diagnostic in failures)
            {
                Console.Error.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
            }

            return new TableEntity[1];
        }

        ms.Seek(0, SeekOrigin.Begin);

        // Load the assembly
        var assembly = Assembly.Load(ms.ToArray());

        // Get types from the assembly and inspect them
        var types = assembly.GetTypes();
        var tableEntityMap = new Dictionary<string, TableEntity>();
        foreach (var type in types)
        {
            var tableEntity = new TableEntity($"{type.FullName}", type.Name);
            tableEntityMap[tableEntity.FullName] = tableEntity;
        }

        foreach (var type in types)
        {
            ProcessType(type, tableEntityMap, PostgreSqlDataTypeMapping.DotNetToPgSqlMapping);
        }

        return tableEntityMap.Values.ToArray();
    }

    internal static void ProcessType(Type type, Dictionary<string, TableEntity> map, Dictionary<string, string> dbTypeMapping)
    {
        var typeName = $"{type.FullName}";
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var attributeColumns = new List<BaseEntityColumn>();
        var refColumns = new List<RefEntityColumn>();

        foreach (PropertyInfo property in properties)
        {
            var propType = property.PropertyType;
            var name = property.Name;
            var isNullable = Nullable.GetUnderlyingType(propType) != null;
            var propTypeStr = isNullable ? $"{Nullable.GetUnderlyingType(propType)}" : $"{propType}";

            if (propType.IsValueType || propType.IsPrimitive)
            {
                var dbType = dbTypeMapping[propTypeStr];
                var col = new BaseEntityColumn(name, dbType, isNullable);
                attributeColumns.Add(col);
            }
            else if (propType == typeof(string) || (isNullable && Nullable.GetUnderlyingType(propType) == typeof(string)))
            {
                var dbType = dbTypeMapping[propTypeStr];
                var length = 256;
                var col = new EntityColumnWithLength(name, dbType, isNullable, length);
                attributeColumns.Add(col);
            }
            else
            {
                var tableEntity = map[propTypeStr];
                RefEntityColumn col = new RefEntityColumn(name, "", isNullable, tableEntity);
                refColumns.Add(col);
                attributeColumns.Add(col);
            }
        }

        var entity = map[typeName];
        entity.SetColumns(attributeColumns, refColumns);
    }

    internal static string GetCreateTableSql(TableEntity[] tableEntities)
    {
        var sb = new StringBuilder();
        var visited = new HashSet<TableEntity>();
        foreach (var entity in tableEntities)
        {
            GetCreateTableSql(entity, sb, visited);
        }

        return sb.ToString();
    }

    internal static void GetCreateTableSql(TableEntity tableEntity, StringBuilder sb, HashSet<TableEntity> visited)
    {
        if (visited.Contains(tableEntity))
        {
            return;
        }

        foreach (var entity in tableEntity.RefEntities)
        {
            GetCreateTableSql(entity, sb, visited);
        }

        sb.Append(tableEntity.GenerateCreateTableSql());
        sb.AppendLine();
        sb.AppendLine();
    }
}