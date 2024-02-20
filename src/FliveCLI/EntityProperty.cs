namespace FliveCLI
{
    public class EntityProperty
    {
        public string PropertyType { get; set; } = default!;
        public string PropertyName { get; set; } = default!;
        public bool IsReferenceType { get; set; }
        public bool IsNullable { get; set; }
        public string DeclaringType { get; set; } = default!;

        public override string ToString()
        {
            return $"Type: {PropertyType}, Name: {PropertyName}, IsReferenceType: {IsReferenceType}, "
                + $"IsNullable: {IsNullable}, DeclaringType: {DeclaringType}";
        }
    }
}