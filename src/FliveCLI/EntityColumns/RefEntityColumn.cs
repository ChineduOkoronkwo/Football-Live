using FliveCLI.TableEntities;

namespace FliveCLI.EntityColumns
{
    public class RefEntityColumn : BaseEntityColumn
    {
        public RefEntityColumn(string columnName, string dbType, bool isNullable, TableEntity refEntity)
        : base(columnName, dbType, isNullable)
        {
            ReferenceTable = refEntity;
        }

        private string GetRefColumnName()
        {
            if (ReferenceTable.PrimaryKeyColumn is null)
            {
                return string.Empty;
            }

            return ReferenceTable.PrimaryKeyColumn.PkColumn.ColumnName.StartsWith(ReferenceTable.Name)
                ? ReferenceTable.PrimaryKeyColumn.PkColumn.ColumnName
                : $"{ReferenceTable.Name}{ReferenceTable.PrimaryKeyColumn.PkColumn.ColumnName}";
        }

        public override string ColumnName => GetRefColumnName();

        public TableEntity ReferenceTable { get; }

        public override string ToTableColumnSql(string columnName = "", string dbType = "")
        {
            if (ReferenceTable.PrimaryKeyColumn is null)
            {
                return string.Empty;
            }

            columnName = GetRefColumnName();
            dbType = ReferenceTable.PrimaryKeyColumn.PkColumn.DbType;
            return base.ToTableColumnSql(columnName, dbType);
        }

        public string GetForeignKeySql()
        {
            if (ReferenceTable.PrimaryKeyColumn is null)
            {
                return string.Empty;
            }

            var refColName = ReferenceTable.PrimaryKeyColumn.PkColumn.ColumnName;
            return $"FOREIGN KEY ({GetRefColumnName()}) REFERENCES {ReferenceTable.Name}({refColName})";
        }
    }
}