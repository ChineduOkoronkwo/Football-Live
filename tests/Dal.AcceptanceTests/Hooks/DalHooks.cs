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
    }
}