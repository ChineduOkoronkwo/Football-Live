using FluentAssertions;
using TechTalk.SpecFlow;

namespace Dal.AcceptanceTests.Steps
{
    [Binding]
    public class RepositoryStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public RepositoryStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("a connection to the (.*) Database with (.*) exists")]
        public void GivenConnectionToDatabase(string rdbmsType, string dbName)
        {
            _scenarioContext["rdbmsType"] = rdbmsType;
            _scenarioContext["dbName"] = dbName;
            rdbmsType.Should().Be("pgsql");
            dbName.Should().Be("BankDB");
        }
    }
}