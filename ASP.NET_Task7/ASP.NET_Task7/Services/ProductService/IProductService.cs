using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Product;

namespace ASP.NET_Task7.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDto> Get(int id);
        Task<bool> Delete(int id);
        Task<ProductDto> Create(CreateProductRequest request);
        Task<PaginatedListDto<ProductDto>> All(PaginationRequest request, string? sortBy, int? categoryId, int? minPrice, int? maxPrice);
    }
}
