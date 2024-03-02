namespace FliveCLI.EntityColumns
{
    public class BaseEntityColumn
    {
        public BaseEntityColumn(string name, string fieldType, string dbType, bool isNullable)
        {
            FieldName = name;
            ColumnName = name.ToLower();
            FieldType = fieldType;
            DbType = dbType;
            IsNullable = isNullable;
        }

        public virtual string ColumnName { get; }
        public virtual string DbType { get; }
        public virtual string FieldName { get; }
        public virtual string FieldType { get; }
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