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

            scenarioContext["customerrepo"] = customerRepo;
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

            scenarioContext["accountrepo"] = accountRepo;
        }

        [When("I use ListAsync on customer repo to read (.*) records starting from record (.*)")]
        public async Task WhenIUseListAsyncOnCustomerRepo(int pageSize, int pageOffset)
        {
            var paginationDto = new PaginationDto { PageSize = pageSize, PageOffset = pageOffset };
            var dataRepo = (IRepository)scenarioContext["customerrepo"];
            var records = await dataRepo.ListAsync<Customer>(paginationDto);
            scenarioContext["customerrecords"] = records;
        }

        [Then("I can verify that there are (.*) customer records which matches the following")]
        public void ThenICanVerifyCustomerRecords(int numRecords, Table table)
        {
            var expectedCustomers = TestUtil.GetCustomersFromTable(table);
            var actualCustomers = (List<Customer>)scenarioContext["customerrecords"];
            actualCustomers.Count.Should().Be(numRecords);
            actualCustomers.Should().BeEquivalentTo(expectedCustomers);
        }

        [When("I use ListAsync on account repo to read (.*) records starting from record (.*)")]
        public async Task WhenIUseListAsyncOnAccountRepo(int pageSize, int pageOffset)
        {
            var paginationDto = new PaginationDto { PageSize = pageSize, PageOffset = pageOffset };
            var dataRepo = (IRepository)scenarioContext["accountrepo"];
            var records = await dataRepo.ListAsync<Account>(paginationDto);
            scenarioContext["accountrecords"] = records;
        }

        [Then("I can verify that there are (.*) account records which matches the following")]
        public void ThenICanVerifyAccountRecords(int numRecords, Table table)
        {
            var expected = TestUtil.GetAccountFromTable(table);
            var actual = (List<Account>)scenarioContext["accountrecords"];
            actual.Count.Should().Be(numRecords);
            expected.Should().BeEquivalentTo(expected);
        }
    }
}