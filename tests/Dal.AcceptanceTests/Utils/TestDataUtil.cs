using Dal.AcceptanceTests.Models;

namespace Dal.AcceptanceTests.Utils
{
    public static class TestDataUtil
    {
        public static async Task LoadCurrencies(string rdbmsType, string dbName)
        {
            var repo = TestUtil.GetCurrencyRepository(rdbmsType, dbName);

            var ngn = new Currency { Id = 1, Code = "NGN", Name = "Nigerian Naira", Territory = "Nigeria" };
            await repo.CreateAsync(ngn);

            var usd = new Currency { Id = 2, Code = "USD", Name = "US Dollar", Territory = "United States of America" };
            await repo.CreateAsync(usd);

            var cad = new Currency { Id = 3, Code = "CAD", Name = "Canadian Dollar", Territory = "Canada" };
            await repo.CreateAsync(cad);

            var gbp = new Currency { Id = 4, Code = "GBP", Name = "British Pounds", Territory = "Great Britain" };
            await repo.CreateAsync(gbp);
        }

        public static async Task LoadAccountTypes(string rdbmsType, string dbName)
        {
            var repo = TestUtil.GetAccountTypeRepository(rdbmsType, dbName);

            var accType = new AccountType { Id = 1, Name = "Savings Account", Description = "Savings Account" };
            await repo.CreateAsync(accType);

            accType = new AccountType { Id = 2, Name = "Current Acct - USD" };
            await repo.CreateAsync(accType);

            accType = new AccountType { Id = 3, Name = "Current Acct - CAD" };
            await repo.CreateAsync(accType);
        }

        public static async Task LoadTransactionTypes(string rdbmsType, string dbName)
        {
            var repo = TestUtil.GetTransactionTypeRepository(rdbmsType, dbName);

            var accType = new TransactionType { Id = 1, Name = "ATM Withdrawal" };
            await repo.CreateAsync(accType);

            accType = new TransactionType { Id = 2, Name = "ATM Deposit" };
            await repo.CreateAsync(accType);

            accType = new TransactionType { Id = 3, Name = "Internet TXFR - DR" };
            await repo.CreateAsync(accType);

            accType = new TransactionType { Id = 4, Name = "Internet TXFR - CR" };
            await repo.CreateAsync(accType);

            accType = new TransactionType { Id = 5, Name = "Bank Deposit" };
            await repo.CreateAsync(accType);

            accType = new TransactionType { Id = 6, Name = "Bank Withdrawal" };
            await repo.CreateAsync(accType);
        }
    }
}