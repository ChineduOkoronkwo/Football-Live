using Dal.Interfaces;

namespace Dal.AcceptanceTests.EntitySql
{
    public class AccountEntitySql : IEntitySqlCommand
    {
        public string GetSqlCommand => "SELECT Id, CurrencyId, AccountTypeId, CustomerId, Balance FROM Account WHERE Id = @Id;";

        public string ListSqlCommand => "SELECT Id, CurrencyId, AccountTypeId, CustomerId, Balance FROM Account"
            + " ORDER BY CurrencyId Limit @PageSize OFFSET @Offset;";

        public string CreateSqlCommand => "INSERT INTO Account(Id, CurrencyId, AccountTypeId, CustomerId, Balance)"
            + " VALUES(@Id, @CurrencyId, @AccountTypeId, @CustomerId, @Balance);";

        public string DeleteSqlCommand => "DELETE FROM Account WHERE Id = @Id;";

        public string UpdateSqlCommand => "UPDATE Account SET CurrencyId=@CurrencyId, AccountTypeId=@AccountTypeId,"
            + " CustomerId=@CustomerId, Balance=@Balance WHERE Id = @Id;";
    }
}