namespace ASP.NET_Task7.Models.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Product>? Products { get; set; }
    }
}
