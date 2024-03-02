namespace FliveCLI.EntityColumns
{
    public class EntityColumnWithLength : BaseEntityColumn
    {
        public EntityColumnWithLength(string columnName, string fieldType, string dbType, bool isNullable, int length)
            : base(columnName, fieldType, dbType, isNullable)
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