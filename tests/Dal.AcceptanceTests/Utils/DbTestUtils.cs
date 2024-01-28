namespace Dal.AcceptanceTests.Utils
{
    public static class DbTestUtils
    {
        public static string? GetDbUserName(string dbName)
            => Environment.GetEnvironmentVariable($"{dbName}TestUserName");
        public static string? GetDbPassword(string dbName)
            => Environment.GetEnvironmentVariable($"{dbName}TestUserPassword");
    }
}