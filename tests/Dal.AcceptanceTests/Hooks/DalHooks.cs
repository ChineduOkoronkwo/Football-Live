using Dal.AcceptanceTests.Utils;
using dotenv.net;
using TechTalk.SpecFlow;

namespace Dal.AcceptanceTests.Hooks
{
    [Binding]
    public static class DalHooks
    {
        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false, probeForEnv: true, probeLevelsToSearch: 4));

            // Load currencies
            await TestDataUtil.LoadCurrencies(TestUtil.PgSqlDbType, TestUtil.TestBankDB);

            // Load account types
            await TestDataUtil.LoadAccountTypes(TestUtil.PgSqlDbType, TestUtil.TestBankDB);

            // Load transaction types
            await TestDataUtil.LoadTransactionTypes(TestUtil.PgSqlDbType, TestUtil.TestBankDB);
        }

        [AfterScenario]
        public static async Task AfterScenario()
        {
            var dapperService = TestUtil.CreateDapperService(TestUtil.PgSqlDbType, TestUtil.TestBankDB);

            // Customer
            await dapperService.ExecuteAsync("Truncate Table Customer Cascade;", null);

            // Account
            await dapperService.ExecuteAsync("Truncate Table Account Cascade;", null);

            // Account Transaction
            await dapperService.ExecuteAsync("Truncate Table AccountTransaction Cascade;", null);
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            var dapperService = TestUtil.CreateDapperService(TestUtil.PgSqlDbType, TestUtil.TestBankDB);

            // Currency
            await dapperService.ExecuteAsync("Truncate Table Currency Cascade;", null);

            // Account Type
            await dapperService.ExecuteAsync("Truncate Table AccountType Cascade;", null);

            // Account Type
            await dapperService.ExecuteAsync("Truncate Table TransactionType Cascade;", null);
        }
    }
}