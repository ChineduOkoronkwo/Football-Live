using System.Globalization;
using Dal.AcceptanceTests.Models;
using Dal.AcceptanceTests.Utils;
using Dal.Interfaces;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Dal.AcceptanceTests.Steps
{
    [Binding]
    public class RepositoryStepDefinitions(ScenarioContext scenarioContext)
    {
        [Given("the (.*) database is hosted on a (.*) rdbms")]
        public void GivenTheDatabaseIsHostedOn(string dbName, string rdbmsType)
        {
            scenarioContext["dbName"] = dbName;
            scenarioContext["rdbmsType"] = rdbmsType;
        }

        [Given("the following customers")]
        public async Task GivenTheFollowingCustomers(Table table)
        {
            var rdbmsType = (string)scenarioContext["rdbmsType"];
            var dbName = (string)scenarioContext["dbName"];
            var customerRepo = TestUtil.GetCustomerRepository(rdbmsType, dbName);

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
                await customerRepo.CreateAsync(customer);
            }
        }

        [Given("their corresponding accounts")]
        public async Task GivenTheirCorrespondingAccounts(Table table)
        {
            var rdbmsType = (string)scenarioContext["rdbmsType"];
            var dbName = (string)scenarioContext["dbName"];
            var accountRepo = TestUtil.GetAccountRepository(rdbmsType, dbName);

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
                await accountRepo.CreateAsync(account);
            }
        }

        [Given("the currency repository with the connection to the (.*) (.*) database")]
        public void GivenDataRepoAndConnectionToDatabase(string rdbmsType, string dbName)
        {
            scenarioContext["currency"] = TestUtil.GetCurrencyRepository(rdbmsType, dbName);
        }

        [When("I use CreateAsync to create currency with (.*), (.*), (.*), (.*)")]
        public async Task WhenIUseCurrencyRepositoryCreateAsync(int id, string code, string name, string territory)
        {
            var currency = new Currency { Id = id, Code = code, Name = name, Territory = territory };
            var repo = (IRepository)scenarioContext["currency"];
            var result = await repo.CreateAsync(currency);
            scenarioContext[$"currency{id}"] = currency;
            result.Should().Be(1);
        }

        [Then("I can use GetAsync to fetch the currency with the (.*)")]
        public async Task ThenIcanUseGetAsyncToFetchTheCurrency(int id)
        {
            var param = new BaseModelId { Id = id };
            var repo = (IRepository)scenarioContext["currency"];
            var actual = await repo.GetAsync<Currency>(param);
            var expected = (Currency)scenarioContext[$"currency{id}"];
            actual.Should().BeEquivalentTo(expected);
        }
    }
}