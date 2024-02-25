using System.Runtime.CompilerServices;
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

            if (PrimaryKeyColumn is not null)
            {
                sb.AppendLine($"\t{PrimaryKeyColumn.ToPkTableColumnSql()},");
            }

            foreach (var col in AttributeColumns)
            {
                sb.AppendLine($"\t{col.ToTableColumnSql()},");
            }

            foreach (var col in ReferenceColumns)
            {
                sb.AppendLine($"\t{col.GetForeignKeySql()},");
            }

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
    }
}