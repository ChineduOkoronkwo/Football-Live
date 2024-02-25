namespace FliveCLI.EntityColumns
{
    public class PkEntityColumn
    {
        public PkEntityColumn(BaseEntityColumn pkColumn)
        {
            PkColumn = pkColumn;
        }

        public BaseEntityColumn PkColumn { get; }
        public string ToPkTableColumnSql()
        {
            return $"{PkColumn.ToShortTableColumnSql()} PRIMARY KEY NOT NULL";
        }
    }
}