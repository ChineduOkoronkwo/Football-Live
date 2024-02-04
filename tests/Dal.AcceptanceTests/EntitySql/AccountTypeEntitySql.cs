using Dal.Interfaces;

namespace Dal.AcceptanceTests.EntitySql
{
    public class AccountTypeEntitySql : IEntitySqlCommand
    {
        public string GetSqlCommand => "SELECT Id, Name, Description FROM AccountType WHERE Id = @Id;";

        public string ListSqlCommand
            => "SELECT Id, Name, Description FROM AccountType ORDER BY Name Limit @PageSize OFFSET @Offset;";

        public string CreateSqlCommand
            => "INSERT INTO AccountType(Id, Name, Description) VALUES(@Id, @Name, @Description);";

        public string DeleteSqlCommand => "DELETE FROM AccountType WHERE Id = @Id;";

        public string UpdateSqlCommand
            => "UPDATE AccountType SET Name = @Name, Description = @Description WHERE Id = @Id;";
    }
}