namespace FliveCLI.EntityColumns
{
    public class BaseEntityColumn
    {
        public BaseEntityColumn(string columnName, string dbType, bool isNullable)
        {
            ColumnName = columnName;
            DbType = dbType;
            IsNullable = isNullable;
        }

        public string ColumnName { get; }
        public string DbType { get; }
        public bool IsNullable { get; }

        public virtual string ToTableColumnSql(string columnName = "", string dbType = "")
        {
            return IsNullable
                ? $"{ToShortTableColumnSql(columnName, dbType)} NULL"
                : $"{ToShortTableColumnSql(columnName, dbType)} NOT NULL";
        }

        public virtual string ToShortTableColumnSql(string columnName = "", string dbType = "")
        {
            columnName = columnName == string.Empty ? ColumnName : columnName;
            dbType = dbType == string.Empty ? DbType : dbType;
            return $"{columnName} {dbType}";
        }
    }
}