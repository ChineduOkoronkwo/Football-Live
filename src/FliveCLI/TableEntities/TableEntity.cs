using System.Text;
using FliveCLI.EntityColumns;

namespace FliveCLI.TableEntities
{
    public class TableEntity
    {
        public TableEntity(string fullName, string name)
        {
            FullName = fullName;
            Name = name;
            RefEntities = new List<TableEntity>();
        }

        public string FullName { get; }
        public string Name { get; }
        public List<BaseEntityColumn> AttributeColumns { get; internal set; } = default!;
        public List<RefEntityColumn> ReferenceColumns { get; internal set; } = default!;
        public List<TableEntity> RefEntities { get; internal set; }
        public PkEntityColumn? PrimaryKeyColumn { get; internal set; }

        public string GenerateCreateTableSql()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE {Name.ToLower()} (");

            // primary key
            if (PrimaryKeyColumn is not null)
            {
                sb.AppendLine($"\t{PrimaryKeyColumn.ToPkTableColumnSql()},");
            }

            // other columns
            foreach (var col in AttributeColumns)
            {
                sb.AppendLine($"\t{col.ToTableColumnSql()},");
            }

            // foreign keys
            foreach (var col in ReferenceColumns)
            {
                sb.AppendLine($"\t{col.GetForeignKeySql()},");
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