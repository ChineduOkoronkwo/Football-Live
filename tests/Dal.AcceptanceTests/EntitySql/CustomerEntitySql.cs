using Dal.Interfaces;

namespace Dal.AcceptanceTests.EntitySql
{
    public class CustomerEntitySql : IEntitySqlCommand
    {
        public string GetSqlCommand => "SELECT Id, FirstName, LastName, MiddleName, DateOfBirth FROM Customer WHERE Id = @Id;";

        public string ListSqlCommand => "SELECT Id, FirstName, LastName, MiddleName, DateOfBirth FROM Customer"
            + " ORDER BY LastName, FirstName Limit @PageSize OFFSET @Offset;";

        public string CreateSqlCommand => "INSERT INTO Customer(Id, FirstName, LastName, MiddleName, DateOfBirth)"
            + " VALUES(@Id, @FirstName, @LastName, @MiddleName, @DateOfBirth);";

        public string DeleteSqlCommand => "DELETE FROM Customer WHERE Id = @Id;";

        public string UpdateSqlCommand => "UPDATE Customer SET FirstName=@FirstName, LastName = @LastName, "
            + "MiddleName = @MiddleName, DateOfBirth = @DateOfBirth WHERE Id = @Id;";
    }
}