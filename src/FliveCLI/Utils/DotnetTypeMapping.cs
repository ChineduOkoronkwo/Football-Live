namespace FliveCLI.Utils
{
    public static class DotnetTypeMapping
    {
        static DotnetTypeMapping()
        {
            TypeMapper = new Dictionary<string, string>
            {
                { "System.Boolean", "bool" },
                { "System.Byte", "byte" },
                { "System.Byte[]", "byte[]" },
                { "System.Data.Spatial.DbGeometry", "geometry" },
                { "System.Data.Spatial.DbGeography", "geography" },
                { "System.DateTime", "DateTime" },
                { "System.DateTimeOffset", "DateTimeOffset" },
                { "System.Decimal", "decimal" },
                { "System.Double", "double" },
                { "System.Guid", "Guid" },
                { "System.Int16", "short" },
                { "System.Int32", "int" },
                { "System.Int64", "long" },
                { "System.SByte", "sbyte" },
                { "System.Single", "float" },
                { "System.String", "string" },
                { "System.TimeSpan", "TimeSpan" },
                { "IEnumerable<System.Int32>", "IEnumerable<int>" },
                { "IEnumerable<System.Guid>", "IEnumerable<Guid>" },
                { "IEnumerable<System.String>", "IEnumerable<string>" }
            };
        }

        public static Dictionary<string, string> TypeMapper { get; set; }
    }
}