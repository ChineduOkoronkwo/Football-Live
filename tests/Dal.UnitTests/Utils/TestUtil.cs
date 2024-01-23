namespace Dal.UnitTests.Utils
{
    public class TestUtil
    {
        protected const string _listSqlCommand = "SELECT * FROM TestEntity WHERE FirstName=@FirstName ORDER BY DateOfBirth DESC Limit @PageSize OFFSET @Offset;";
        protected const string _getSqlCommand = "SELECT * FROM TestEntity WHERE Id=@Id;";
        protected const string _createSqlCommand = "INSERT INTO TestEntity(Id, FirstName, LastName, DateOfBirth) VALUES(@Id, @FirstName, @LastName, @DateOfBirth);";
        protected const string _updateSqlCommand = "UPDATE TestEntity SET FirstName=@FirstName, LastName=@LastName, DateOfBirth=@DateOfBirth WHERE Id=@Id;";
        protected const string _deleteSqlCommand = "Delete FROM TestEntity WHERE Id=@Id;";
    }
}