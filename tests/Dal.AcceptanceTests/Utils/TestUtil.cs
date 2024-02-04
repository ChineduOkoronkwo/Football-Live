using System.Data;
using System.Globalization;
using Dal.AcceptanceTests.EntitySql;
using Dal.AcceptanceTests.Models;
using Dal.Interfaces;
using Dal.Services;
using Npgsql;
using TechTalk.SpecFlow;

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

        public static List<Customer> GetCustomersFromTable(Table table)
        {
            var customers = new List<Customer>();
            foreach (var row in table.Rows)
            {
                var customer = new Customer
                {
                    Id = int.Parse(row["Id"]),
                    FirstName = row["FirstName"],
                    LastName = row["LastName"],
                    Middlename = row["MiddleName"],
                    DateOfBirth = DateTime.Parse(row["DateOfBirth"], new CultureInfo("en-US"))
                };
                customers.Add(customer);
            }

            return customers;
        }

        public static List<Account> GetAccountFromTable(Table table)
        {
            var accounts = new List<Account>();
            foreach (var row in table.Rows)
            {
                var account = new Account
                {
                    Id = int.Parse(row["Id"]),
                    CurrencyId = int.Parse(row["CurrencyId"]),
                    AccountTypeId = int.Parse(row["AccountTypeId"]),
                    CustomerId = int.Parse(row["CustomerId"]),
                    Balance = int.Parse(row["Balance"]),
                };
                accounts.Add(account);
            }

            return accounts;
        }
    }
}