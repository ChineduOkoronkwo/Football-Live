namespace FliveCLI.EntityColumns
{
    public class EntityColumnWithLength : BaseEntityColumn
    {
        public EntityColumnWithLength(string columnName, string dbType, bool isNullable, int length)
            : base(columnName, dbType, isNullable)
        {
            Length = length;
        }

        public int Length { get; }

        public override string ToTableColumnSql(string columnName = "", string dbType = "")
        {
            return base.ToTableColumnSql(columnName, $"{DbType}({Length})");
        }
    }
}