namespace FliveCLI
{
    public class PostgreSqlDataTypeMapping
    {
        public PostgreSqlDataTypeMapping()
        {
            DotNetToPgSqlMapping = new Dictionary<string, string>
            {
                { "System.Boolean", "boolean" },
                { "System.Byte", "smallint" },
                { "System.Byte[]", "bytea" },
                { "System.Data.Spatial.DbGeometry", "geometry" },
                { "System.Data.Spatial.DbGeography", "geography" },
                { "System.DateTime", "timestamp" },
                { "System.DateTimeOffset", "timestamp with time zone" },
                { "System.Decimal", "numeric" },
                { "System.Double", "double precision" },
                { "System.Guid", "uuid" },
                { "System.Int16", "smallint" },
                { "System.Int32", "integer" },
                { "System.Int64", "bigint" },
                { "System.SByte", "smallint" },
                { "System.Single", "real" },
                { "System.String", "varchar" },
                { "System.String2", "char" },
                { "System.TimeSpan", "timestamp" }
            };
        }

        public Dictionary<string, string> DotNetToPgSqlMapping { get; set; } = default!;
    }
}