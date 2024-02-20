using System.CommandLine;
using System.Reflection;
using FliveCLI;
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

            return;
        }

        ms.Seek(0, SeekOrigin.Begin);

        // Load the assembly
        var assembly = Assembly.Load(ms.ToArray());

        // Get types from the assembly and inspect them
        var types = assembly.GetTypes();
        var entityInfos = new List<EntityInfo>();
        foreach (var type in types)
        {
            var entityInfo = new EntityInfo { FullName = $"{type.FullName}", Name = type.Name, Properties = new List<EntityProperty>() };
            entityInfos.Add(entityInfo);

            // Inspect public properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Get property infos
            foreach (PropertyInfo property in properties)
            {
                var entityProperty = new EntityProperty
                {
                    PropertyType = $"{property.PropertyType}",
                    PropertyName = property.Name,
                    IsReferenceType = IsReferenceType(property.PropertyType),
                    IsNullable = IsPropertyNullable(property),
                    DeclaringType = $"{property.DeclaringType}"
                };
                entityInfo.Properties.Add(entityProperty);
            }
        }
    }

    private static bool IsReferenceType(Type type)
    {
        return !type.IsValueType && !type.IsPrimitive && type != typeof(string);
    }

    private static bool IsPropertyNullable(PropertyInfo property)
    {
        Type propType = property.PropertyType;
        return Nullable.GetUnderlyingType(propType) != null;
    }
}