namespace FliveCLI.Utils
{
    public static class PostgreSqlDataTypeMapping
    {
        static PostgreSqlDataTypeMapping()
        {
            DotNetToPgSqlMapping = new Dictionary<string, string>
            {
                { "System.Boolean", "BOOLEAN" },
                { "System.Byte", "SMALLINT" },
                { "System.Byte[]", "BYTEA" },
                { "System.Data.Spatial.DbGeometry", "geometry" },
                { "System.Data.Spatial.DbGeography", "geography" },
                { "System.DateTime", "TIMESTAMP" },
                { "System.DateTimeOffset", "TIMESTAMP WITH TIME ZONE" },
                { "System.Decimal", "NUMERIC" },
                { "System.Double", "DOUBLE PRECISION" },
                { "System.Guid", "UUID" },
                { "System.Int16", "SMALLINT" },
                { "System.Int32", "INTEGER" },
                { "System.Int64", "BIGINTEGER" },
                { "System.SByte", "SMALLINT" },
                { "System.Single", "REAL" },
                { "System.String", "VARCHAR" },
                { "System.String2", "CHAR" },
                { "System.TimeSpan", "TIMESTAMP" },
                { "System.Nullable`1[System.Int32]", "INTEGER" },
                { "System.Nullable`1[System.DateTime]", "TIMESTAMP" }
            };
        }

        public static Dictionary<string, string> DotNetToPgSqlMapping { get; set; } = default!;
    }
}