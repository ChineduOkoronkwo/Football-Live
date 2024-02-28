using System.Text;
using FliveCLI.EntityColumns;
using FliveCLI.Helpers;

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

        public SqlStatementList GenerateGetSql()
        {
            var cols = GetColumnNames();
            return new SqlStatementList
            {
                $"SELECT {string.Join(", ", cols)}",
                $"FROM {Name}",
                $"{GetDefaultWhereClause()};"
            };
        }

        public SqlStatementList GenerateListSql()
        {
            var cols = GetColumnNames();
            var filterSqlList = new List<string>();
            var pkColumName = PrimaryKeyColumn?.PkColumn.ColumnName;
            var orderByClause = "";
            if (!string.IsNullOrWhiteSpace(pkColumName))
            {
                filterSqlList.Add($"AND (@{pkColumName} IS NULL OR {pkColumName} = @{pkColumName})");
                orderByClause = $"ORDER BY {pkColumName} ASC";
            }

            ReferenceColumns.ForEach(col => filterSqlList.Add(
                $"AND (@{col.ColumnName} IS NULL OR {col.ColumnName} = @{col.ColumnName})"
            ));

            if (filterSqlList.Count > 0)
            {
                filterSqlList[0] = filterSqlList[0].Replace("AND ", "WHERE ");
            }

            var limitClause = "LIMIT @pagesize OFFSET @pageoffset;";
            var sqlList = new SqlStatementList
            {
                $"SELECT {string.Join(", ", cols)}",
                $"FROM {Name}"
            };

            foreach (var sql in filterSqlList)
            {
                sqlList.Add(sql);
            }

            sqlList.Add(orderByClause);
            sqlList.Add(limitClause);

            return sqlList;
        }

        public SqlStatementList GenerateDeleteSql()
        {
            return
            [
                $"DELETE FROM {Name}",
                $"{GetDefaultWhereClause()};"
            ];
        }

        public SqlStatementList GenerateCreateSql()
        {
            var cols = GetColumnNames();
            return
            [
                $"INSERT INTO {Name}({string.Join(", ", cols)})",
                $"VALUES(@{string.Join(", @", cols)});"
            ];
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

        private string GetDefaultWhereClause()
        {
            var pkColumName = PrimaryKeyColumn?.PkColumn.ColumnName;
            return string.IsNullOrWhiteSpace(pkColumName) ? "" : $"WHERE {pkColumName} = @{pkColumName}";
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