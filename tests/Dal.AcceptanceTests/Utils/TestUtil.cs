using System.Data;
using Dal.AcceptanceTests.EntitySql;
using Dal.Interfaces;
using Dal.Services;
using Npgsql;

namespace Dal.AcceptanceTests.Utils
{
    public static class TestUtil
    {
        public static string PgSqlDbType => "pgsql";

        private static IDbConnection GetDbConnection(string rdbmsType, string dbName)
        {
            var username = Environment.GetEnvironmentVariable("TestBankDBUsername");
            var pswd = Environment.GetEnvironmentVariable("TestBankDBPassword");
            var connectionString = "";
            if (rdbmsType == PgSqlDbType)
            {
                connectionString = $"Server=localhost; Port=5432; User Id={username}; Password={pswd}; Database={dbName}";
            }

            return new NpgsqlConnection(connectionString);
        }

        public static IRepository GetCurrencyRepository(string rdbmsType, string dbName)
        {
            var dapperService = new DapperService(GetDbConnection(rdbmsType, dbName));
            return new Repository(dapperService, new CurrencyEntitySql());
        }
    }
}