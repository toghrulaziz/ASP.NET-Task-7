using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Task7.Models.DTOs.Product
{
    public class CreateProductRequest
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(5)]
        public string Description { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
