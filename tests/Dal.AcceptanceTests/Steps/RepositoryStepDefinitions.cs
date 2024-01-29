using Dal.AcceptanceTests.Models;
using Dal.AcceptanceTests.Utils;
using Dal.Interfaces;
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

        [Given("the currency repository with the connection to the (.*) (.*) database")]
        public void GivenDataRepoAndConnectionToDatabase(string rdbmsType, string dbName)
        {
            _scenarioContext["currency"] = TestUtil.GetCurrencyRepository(rdbmsType, dbName);
        }

        [When("I use CreateAsync to create currency with (.*), (.*), (.*), (.*)")]
        public async Task WhenIUseCurrencyRepositoryCreateAsync(int id, string code, string name, string territory)
        {
            var currency = new Currency { Id = id, Code = code, Name = name, Territory = territory };
            var repo = (IRepository)_scenarioContext["currency"];
            var result = await repo.CreateAsync(currency);
            _scenarioContext[$"currency{id}"] = currency;
            result.Should().Be(1);
        }

        [Then("I can use GetAsync to fetch the currency with the (.*)")]
        public async Task ThenIcanUseGetAsyncToFetchTheCurrency(int id)
        {
            var param = new BaseModelId { Id = id };
            var repo = (IRepository)_scenarioContext["currency"];
            var actual = await repo.GetAsync<Currency>(param);
            var expected = (Currency)_scenarioContext[$"currency{id}"];
            actual.Should().BeEquivalentTo(expected);
        }
    }
}