using System.Collections.ObjectModel;
using System.Reflection;
using FliveCLI.EntityColumns;
using FliveCLI.TableEntities;
using FliveCLI.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FliveCLI.EntityFileProcessors
{
    public static class TypeLoader
    {
        internal static Type[] LoadTypesFromFile(string dirPath)
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

                return new Type[1];
            }

            ms.Seek(0, SeekOrigin.Begin);

            // Load the assembly
            var assembly = Assembly.Load(ms.ToArray());
            return assembly.GetTypes();
        }

        internal static TableEntity[] CreateTableEntitiesFromType(Type[] types)
        {
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

        private static void ProcessType(Type type, Dictionary<string, TableEntity> map, Dictionary<string, string> dbTypeMapping)
        {
            var typeName = $"{type.FullName}";
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var attributeColumns = new List<BaseEntityColumn>();
            var refColumns = new List<RefEntityColumn>();
            var entity = map[typeName];

            foreach (PropertyInfo property in properties)
            {
                var propType = property.PropertyType;
                var name = property.Name;
                var isNullable = IsNullableHelper(propType, property.DeclaringType, property.CustomAttributes);
                var propTypeStr = isNullable && propType.IsValueType ? $"{Nullable.GetUnderlyingType(propType)}" : $"{propType}";

                if (propType.IsValueType || propType.IsPrimitive)
                {
                    var dbType = dbTypeMapping[propTypeStr];
                    var col = new BaseEntityColumn(name, propTypeStr, dbType, isNullable);
                    attributeColumns.Add(col);
                }
                else if (propType == typeof(string) || (isNullable && Nullable.GetUnderlyingType(propType) == typeof(string)))
                {
                    var dbType = dbTypeMapping[propTypeStr];
                    var length = 256;
                    var col = new EntityColumnWithLength(name, propTypeStr, dbType, isNullable, length);
                    attributeColumns.Add(col);
                }
                else
                {
                    var refTablEntity = map[propTypeStr];
                    RefEntityColumn col = new RefEntityColumn(name, propTypeStr, propTypeStr, isNullable, refTablEntity);
                    refColumns.Add(col);
                    attributeColumns.Add(col);
                    entity.RefEntities.Add(refTablEntity);
                }
            }

            entity.SetColumns(attributeColumns, refColumns);
        }

        private static bool IsNullableHelper(Type memberType, MemberInfo? declaringType, IEnumerable<CustomAttributeData> customAttributes)
        {
            var nullableAttributeFullName = "System.Runtime.CompilerServices.NullableAttribute";
            if (memberType.IsValueType)
                return Nullable.GetUnderlyingType(memberType) != null;

            var nullable = customAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == nullableAttributeFullName);
            if (nullable != null && nullable.ConstructorArguments.Count == 1)
            {
                var attributeArgument = nullable.ConstructorArguments[0];
                if (attributeArgument.ArgumentType == typeof(byte[]))
                {
                    var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
                    if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                    {
                        return (byte)args[0].Value! == 2;
                    }
                }
                else if (attributeArgument.ArgumentType == typeof(byte))
                {
                    return (byte)attributeArgument.Value! == 2;
                }
            }

            for (var type = declaringType; type != null; type = type.DeclaringType)
            {
                var context = type.CustomAttributes
                    .FirstOrDefault(x => x.AttributeType.FullName == nullableAttributeFullName);
                if (context != null &&
                    context.ConstructorArguments.Count == 1 &&
                    context.ConstructorArguments[0].ArgumentType == typeof(byte))
                {
                    return (byte)context.ConstructorArguments[0].Value! == 2;
                }
            }

            return false;
        }
    }
}