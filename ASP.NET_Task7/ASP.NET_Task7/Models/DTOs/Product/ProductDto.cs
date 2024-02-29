using ASP.NET_Task7.Models.Entities;

namespace ASP.NET_Task7.Models.DTOs.Product
{
    public class ProductDto
    {
        public ProductDto(int id, string name, string description, decimal price, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }

        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; } 
        public int CategoryId { get; set; } 
    }
}
