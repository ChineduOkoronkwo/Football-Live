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
            var customers = TestUtil.GetCustomersFromTable(table);

            foreach (var customer in customers)
            {
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
            var accounts = TestUtil.GetAccountFromTable(table);

            foreach (var account in accounts)
            {
                await accountRepo.CreateAsync(account);
            }

            scenarioContext["accountrepo"] = accountRepo;
        }

        [Given("The ListSqlCommand of (.*) is set to order records by (.*)")]
        public void GivenTheListSqlCommandHasOrderBy(string entitySql, string orderby)
        {
            scenarioContext[$"{entitySql}orderby"] = orderby;
        }

        [When("I use ListAsync on customer repo to read (.*) records starting from record (.*)")]
        public async Task WhenIUseListAsyncOnCustomerRepo(int pageSize, int pageOffset)
        {
            var paginationDto = new PaginationDto { PageSize = pageSize, PageOffset = pageOffset };
            var dataRepo = (IRepository)scenarioContext["customerrepo"];
            var records = await dataRepo.ListAsync<Customer>(paginationDto);
            scenarioContext["customerrecords"] = records;
        }

        [When("I use ListAsync on account repo to read (.*) records starting from record (.*)")]
        public async Task WhenIUseListAsyncOnAccountRepo(int pageSize, int pageOffset)
        {
            var paginationDto = new PaginationDto { PageSize = pageSize, PageOffset = pageOffset };
            var dataRepo = (IRepository)scenarioContext["accountrepo"];
            var records = await dataRepo.ListAsync<Account>(paginationDto);
            scenarioContext["accountrecords"] = records;
        }

        [When("I update the MiddleName to (.*) and FirstName to (.*) for Customer (.*)")]
        public async Task UpdateMiddleAndFirstName(string middlename, string firstName, int id)
        {
            var param = new BaseModelId { Id = id };
            var customerRepo = (IRepository)scenarioContext["customerrepo"];
            var customer = await customerRepo.GetAsync<Customer>(param);
            customer.FirstName = firstName;
            customer.Middlename = middlename;
            var numRecords = await customerRepo.UpdateAsync(customer);
            numRecords.Should().Be(1);
        }

        [Then("I can verify that there are (.*) customer records which matches the following")]
        public void ThenICanVerifyCustomerRecords(int numRecords, Table table)
        {
            var expectedCustomers = TestUtil.GetCustomersFromTable(table);
            var actualCustomers = (List<Customer>)scenarioContext["customerrecords"];
            actualCustomers.Count.Should().Be(numRecords);

            var map = new Dictionary<int, Customer>();
            foreach (var customer in expectedCustomers)
            {
                map[customer.Id] = customer;
            }

            foreach (var customer in actualCustomers)
            {
                Assert.That(map.ContainsKey(customer.Id), Is.True);
                map[customer.Id].Should().BeEquivalentTo(customer);
            }
        }

        [Then("I can verify that there are (.*) account records which matches the following")]
        public void ThenICanVerifyAccountRecords(int numRecords, Table table)
        {
            var expected = TestUtil.GetAccountFromTable(table);
            var actual = (List<Account>)scenarioContext["accountrecords"];
            actual.Count.Should().Be(numRecords);
            var map = new Dictionary<int, Account>();
            foreach (var account in expected)
            {
                map[account.Id] = account;
            }

            foreach (var account in actual)
            {
                Assert.That(map.ContainsKey(account.Id), Is.True);
                map[account.Id].Should().BeEquivalentTo(account);
            }
        }

        [Then("I can verify that the changes to customer (.*) are persisted")]
        public async Task ThenICanVerifyChangesToCustomer(int id, Table table)
        {
            var expectedCustomer = TestUtil.GetCustomersFromTable(table);
            expectedCustomer.Count.Should().Be(1);

            var param = new BaseModelId { Id = id };
            var customerRepo = (IRepository)scenarioContext["customerrepo"];
            var actualCustomer = await customerRepo.GetAsync<Customer>(param);
            actualCustomer.Should().BeEquivalentTo(expectedCustomer[0]);
        }
    }
}