namespace FliveCLI.EntityColumns
{
    public class ListDtoColumn : BaseEntityColumn
    {
        public ListDtoColumn(string name, string fieldType, string dbType) : base(name, fieldType, dbType, true)
        {
        }

        public override string ColumnName => base.ColumnName + "list";
        public override string FieldName => base.FieldName + "list";
        public override string FieldType => $"IEnumerable<{base.FieldType}>";
    }
}