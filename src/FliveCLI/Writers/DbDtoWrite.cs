using System.Text;
using FliveCLI.EntityColumns;
using FliveCLI.TableEntities;
using FliveCLI.Utils;

namespace FliveCLI.Writers
{
    public static class DbDtoWrite
    {
        internal static string Foldername => "DbDtos";
        internal static void Write(TableEntity tableEntity,  HashSet<string> createdDtos, string projectName, string namespaceName = "Dal.Dtos")
        {
            WriteDtos(tableEntity.EntityColumns, tableEntity.TEntityName, projectName, namespaceName);

            // Dto fields for Get and Delete params
            var cols = new List<BaseEntityColumn>();
            if (tableEntity.PrimaryKeyColumn is not null)
            {
                var dtoName = tableEntity.TGetParamEntityName;
                cols.Add(tableEntity.PrimaryKeyColumn.PkColumn);
                if (!createdDtos.Contains(dtoName))
                {
                    WriteDtos(cols, dtoName, projectName, namespaceName);
                    createdDtos.Add(dtoName);
                }
            }

            // Pagination Dto
            var paginationDtoName = "PaginationDTO";
            if (!createdDtos.Contains(paginationDtoName))
            {
                WritePaginationDto(paginationDtoName, projectName, namespaceName);
                createdDtos.Add(paginationDtoName);
            }

            // List Dto
            WriteDtos(tableEntity.ListDtoFilterColumns, tableEntity.TListParamEntityName, projectName, namespaceName, " : " + paginationDtoName);
        }

        private static void WriteDtos(List<BaseEntityColumn> entityColumns, string dtoName, string projectName, string namespaceName, string baseClassSuffix = "")
        {
            var usingStatements = new HashSet<string>();
            var classFields = new StringBuilder();
            ProcessFields(entityColumns, usingStatements, classFields);

            var fullPath = FileUtil.GetPath(projectName, Foldername, dtoName + ".cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            foreach (var item in usingStatements)
            {
                writer.WriteLine($"using {item};");
            }

            if (usingStatements.Count > 0)
            {
                writer.WriteLine();
            }

            if (!string.IsNullOrWhiteSpace(namespaceName))
            {
                writer.WriteLine($"namespace {namespaceName};");
                writer.WriteLine();
            }

            writer.WriteLine($"public class {dtoName}{baseClassSuffix}");
            writer.WriteLine("{");
            writer.Write(classFields.ToString());
            writer.WriteLine("}");
        }

        private static void WritePaginationDto(string dtoName, string projectName, string namespaceName)
        {
            var fullPath = FileUtil.GetPath(projectName, Foldername, dtoName + ".cs");
            using StreamWriter writer = new StreamWriter(fullPath, true);
            writer.WriteLine("using System;");
            writer.WriteLine();
            if (!string.IsNullOrWhiteSpace(namespaceName))
            {
                writer.WriteLine($"namespace {namespaceName};");
                writer.WriteLine();
            }

            writer.WriteLine($"public class {dtoName}");
            writer.WriteLine("{");
            writer.WriteLine("    public int PageSize { get; set; } = 1000;");
            writer.WriteLine("    public int PageOffset { get; set; }");
            writer.WriteLine("}");
        }

        private static void ProcessFields(List<BaseEntityColumn> entityColumns, HashSet<string> usingStatements, StringBuilder classFields)
        {
            foreach (var col in entityColumns)
            {
                if (col.FieldType.StartsWith("IEnumerable"))
                {
                    usingStatements.Add("System.Collections.Generic");
                }
                else
                {
                    var lastDotIndex = col.FieldType.LastIndexOf('.');
                    if (lastDotIndex > 0)
                    {
                        usingStatements.Add(col.FieldType[..lastDotIndex]);
                    }
                }

                var nullable = col.IsNullable ? "?" : "";
                var fieldType = DotnetTypeMapping.TypeMapper[col.FieldType];
                var defaultValue = !col.IsNullable && fieldType.Equals("string") ? " = default!;" : "";
                classFields.AppendLine($"    public {fieldType}{nullable} {col.FieldName} {{ get; set; }}{defaultValue}");
            }
        }
    }
}