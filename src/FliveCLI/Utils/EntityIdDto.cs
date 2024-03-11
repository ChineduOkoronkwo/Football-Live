namespace FliveCLI.Utils
{
    public static class EntityIdDto
    {
        static EntityIdDto()
        {
            EntityIdDtoMapper = new Dictionary<string, string>
            {
                { "System.Boolean", "Bool" },
                { "System.Byte", "Byte" },
                { "System.DateTime", "DateTime" },
                { "System.DateTimeOffset", "DateTimeOffset" },
                { "System.Decimal", "Decimal" },
                { "System.Double", "Double" },
                { "System.Guid", "Guid" },
                { "System.Int16", "Short" },
                { "System.Int32", "Int" },
                { "System.Int64", "Long" },
                { "System.SByte", "SByte" },
                { "System.Single", "Float" },
                { "System.String", "String" },
                { "System.TimeSpan", "TimeSpan" },
            };
        }

        public static Dictionary<string, string> EntityIdDtoMapper { get; set; }
    }
}