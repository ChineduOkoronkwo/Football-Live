using System.Text;
using FliveCLI.EntityColumns;

namespace FliveCLI.TableEntities
{
    public class TableEntity
    {
        public TableEntity(string fullName, string name)
        {
            FullName = fullName;
            Name = name.ToLower();
            RefEntities = new List<TableEntity>();
        }

        public string FullName { get; }
        public string Name { get; }
        public List<BaseEntityColumn> AttributeColumns { get; internal set; } = default!;
        public List<RefEntityColumn> ReferenceColumns { get; internal set; } = default!;
        public List<TableEntity> RefEntities { get; internal set; }
        public PkEntityColumn? PrimaryKeyColumn { get; internal set; }

        public string GenerateGetSql()
        {
            var cols = GetColumnNames();
            var pkColumName = PrimaryKeyColumn?.PkColumn.ColumnName;
            var whereClause = string.IsNullOrWhiteSpace(pkColumName) ? "" : $"\nWHERE {pkColumName} = @{pkColumName}";

            return $"SELECT {string.Join(", ", cols)} \nFROM {Name}{whereClause};";
        }

        public string GenerateListSql()
        {
            var cols = GetColumnNames();
            var filtercols = new List<string>();
            var pkColumName = PrimaryKeyColumn?.PkColumn.ColumnName;
            var orderByClause = "";
            if (!string.IsNullOrWhiteSpace(pkColumName))
            {
                filtercols.Add($"(@{pkColumName} IS NULL OR {pkColumName} = @{pkColumName})");
                orderByClause = $"\nORDER BY {pkColumName} ASC";
            }

            foreach (var col in ReferenceColumns)
            {
                var refColName = col.GetRefColumnName();
                filtercols.Add($"(@{refColName} IS NULL OR {refColName} = @{refColName})");
            }

            var limitClause = "\nLIMIT @pagesize OFFSET @pageoffset;";
            var whereClause = filtercols.Count == 0 ? "" : $"\nWHERE {string.Join("\nAND", filtercols)}";
            return $"SELECT {string.Join(", ", cols)}\nFROM {Name}{whereClause}{orderByClause}{limitClause}";
        }

        public string GenerateCreateTableSql()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE {Name} (");

            // primary key
            if (PrimaryKeyColumn is not null)
            {
                sb.AppendLine($"{PrimaryKeyColumn.ToPkTableColumnSql()},");
            }

            // other columns
            foreach (var col in AttributeColumns)
            {
                sb.AppendLine($"{col.ToTableColumnSql()},");
            }

            // foreign keys
            foreach (var col in ReferenceColumns)
            {
                sb.AppendLine($"{col.GetForeignKeySql()},");
            }

            ReplaceLastChar(sb, ',', '\0');
            sb.AppendLine(");");

            return sb.ToString();
        }

        public void SetColumns(List<BaseEntityColumn> attributeColumns, List<RefEntityColumn> referenceColumns)
        {
            AttributeColumns = attributeColumns;
            ReferenceColumns = referenceColumns;
            UpdatePkEntity();
        }

        private void UpdatePkEntity()
        {
            BaseEntityColumn? pkColum =
            AttributeColumns.Find(tc => tc.ColumnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                ?? AttributeColumns.Find(tc => tc.ColumnName.Equals($"{Name}id", StringComparison.OrdinalIgnoreCase))
                ?? AttributeColumns.Find(tc => tc.ColumnName.Equals($"{Name}_id", StringComparison.OrdinalIgnoreCase));
            if (pkColum is not null)
            {
                PrimaryKeyColumn = new PkEntityColumn(pkColum);
                AttributeColumns.Remove(pkColum);
            }
        }

        private List<string> GetColumnNames(bool includePkColumn = true)
        {
            var entityColumns = new List<string>();
            var pkColName = PrimaryKeyColumn?.PkColumn.ColumnName;
            if (includePkColumn && !string.IsNullOrWhiteSpace(pkColName))
            {
                entityColumns.Add(pkColName);
            }

            foreach (var col in AttributeColumns)
            {
                entityColumns.Add(col.ColumnName);
            }

            return entityColumns;
        }

        private static void ReplaceLastChar(StringBuilder sb, char oldChar, char newChar)
        {
            for (int index = sb.Length - 1; index > -1; index--)
            {
                if (sb[index] == oldChar)
                {
                    sb[index] = newChar;
                    return;
                }
            }
        }
    }
}