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
        public static string TestBankDB => "testbankdb";

        private static IDbConnection GetDbConnection(string rdbmsType, string dbName)
        {
            var username = Environment.GetEnvironmentVariable("TestBankDBUsername");
            var pswd = Environment.GetEnvironmentVariable("TestBankDBPassword");
            var connectionString = "";
            if (rdbmsType == PgSqlDbType)
            {
                connectionString = $"Server=localhost; Port=5432; User Id={username}; Password={pswd}; Database={dbName}; Pooling=true; Include Error Detail=true";
            }

            return new NpgsqlConnection(connectionString);
        }

        public static IRepository GetCurrencyRepository(string rdbmsType, string dbName)
        {
            return new Repository(CreateDapperService(rdbmsType, dbName), new CurrencyEntitySql());
        }

        public static IRepository GetAccountTypeRepository(string rdbmsType, string dbName)
        {
            return new Repository(CreateDapperService(rdbmsType, dbName), new AccountTypeEntitySql());
        }

        public static IRepository GetTransactionTypeRepository(string rdbmsType, string dbName)
        {
            return new Repository(CreateDapperService(rdbmsType, dbName), new TransactionTypeEntitySql());
        }

        public static IDapperService CreateDapperService(string rdbmsType, string dbName)
        {
            return new DapperService(GetDbConnection(rdbmsType, dbName));
        }

        public static IRepository GetCustomerRepository(string rdbmsType, string dbName)
        {
            return new Repository(CreateDapperService(rdbmsType, dbName), new CustomerEntitySql());
        }

        public static IRepository GetAccountRepository(string rdbmsType, string dbName)
        {
            return new Repository(CreateDapperService(rdbmsType, dbName), new AccountEntitySql());
        }
    }
}