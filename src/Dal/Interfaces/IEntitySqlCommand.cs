namespace Dal.Interfaces
{
    public interface IEntitySqlCommand
    {
        string GetSqlCommand { get; }
        string ListSqlCommand { get; }
        string CreateSqlCommand { get; }
        string DeleteSqlCommand { get; }
        string UpdateSqlCommand { get; }
    }
}