using Dal.AcceptanceTests.Utils;
using dotenv.net;
using TechTalk.SpecFlow;

namespace Dal.AcceptanceTests.Hooks
{
    [Binding]
    public static class DalHooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false, probeForEnv: true, probeLevelsToSearch: 4));
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            var dapperService = TestUtil.CreateDapperService(TestUtil.PgSqlDbType, TestUtil.TestBankDB);
            await dapperService.ExecuteAsync("Truncate Table Currency Cascade;", null);
        }
    }
}