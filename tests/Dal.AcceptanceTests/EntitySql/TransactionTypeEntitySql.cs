using Dal.Interfaces;

namespace Dal.AcceptanceTests.EntitySql
{
    public class TransactionTypeEntitySql : IEntitySqlCommand
    {
        public string GetSqlCommand => "SELECT Id, Name, Description FROM TransactionType WHERE Id = @Id;";

        public string ListSqlCommand
            => "SELECT Id, Name, Description FROM TransactionType ORDER BY Name Limit @PageSize OFFSET @Offset;";

        public string CreateSqlCommand
            => "INSERT INTO TransactionType(Id, Name, Description) VALUES(@Id, @Name, @Description);";

        public string DeleteSqlCommand => "DELETE FROM TransactionType WHERE Id = @Id;";

        public string UpdateSqlCommand
            => "UPDATE TransactionType SET Name = @Name, Description = @Description WHERE Id = @Id;";
    }
}