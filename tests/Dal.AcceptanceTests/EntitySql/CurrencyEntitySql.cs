using Dal.Interfaces;

namespace Dal.AcceptanceTests.EntitySql
{
    public class CurrencyEntitySql : IEntitySqlCommand
    {
        public string GetSqlCommand => "SELECT Id, Code, Name, Territory FROM Currency WHERE Id = @Id;";

        public string ListSqlCommand
            => "SELECT Id, Code, Name, Territory FROM Currency ORDER BY Name LIMIT @PageSize OFFSET @PageOffset;";

        public string CreateSqlCommand
            => "INSERT INTO Currency(Id, Code, Name, Territory) VALUES(@Id, @Code, @Name, @Territory);";

        public string DeleteSqlCommand => "DELETE FROM Currency WHERE Id = @Id;";

        public string UpdateSqlCommand
            => "UPDATE Currency SET Code = @Code, Name = @Name, Territory = @Territory WHERE Id = @Id;";
    }
}