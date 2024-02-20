namespace FliveCLI
{
    public class EntityInfo
    {
        public string FullName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<EntityProperty> Properties { get; set; } = default!;
    }
}