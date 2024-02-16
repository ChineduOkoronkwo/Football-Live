using Dal.Interfaces;

namespace Dal.AcceptanceTests.EntitySql
{
    public class AccountEntitySql : IEntitySqlCommand
    {
        public string GetSqlCommand
            => "SELECT Id, CurrencyId, AccountTypeId, CustomerId, Name, Description, Balance FROM Account WHERE Id = @Id;";

        public string ListSqlCommand =>
            "SELECT Id, CurrencyId, AccountTypeId, CustomerId, Name, Description, Balance"
            + " FROM Account"
            + " WHERE (@CustomerId IS NULL OR CustomerId = @CustomerId)"
            + " AND (@CurrencyId IS NULL OR CurrencyId = @CurrencyId)"
            + " AND (@AccountTypeId IS NULL OR AccountTypeId = @AccountTypeId)"
            + " ORDER BY CurrencyId, Name, AccountTypeId LIMIT @PageSize OFFSET @PageOffset;";

        public string CreateSqlCommand => "INSERT INTO Account(Id, CurrencyId, AccountTypeId, CustomerId, Name, Description, Balance)"
            + " VALUES(@Id, @CurrencyId, @AccountTypeId, @CustomerId, @Name, @Description, @Balance);";

        public string DeleteSqlCommand => "DELETE FROM Account WHERE Id = @Id;";

        public string UpdateSqlCommand => "UPDATE Account SET CurrencyId=@CurrencyId, AccountTypeId=@AccountTypeId,"
            + " Name=@Name, Description=@Description, CustomerId=@CustomerId, Balance=@Balance WHERE Id = @Id;";
    }
}