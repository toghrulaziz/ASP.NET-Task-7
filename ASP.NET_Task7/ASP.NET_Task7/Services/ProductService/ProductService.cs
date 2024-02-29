using ASP.NET_Task7.Data;
using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Product;
using ASP.NET_Task7.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task7.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly TodoDbContext _context;

        public ProductService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedListDto<ProductDto>> All(PaginationRequest request, string? sortBy, int? categoryId, int? minPrice, int? maxPrice)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category);

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(p => p.Name);
                        break;
                    case "price":
                        query = query.OrderBy(p => p.Price);
                        break;
                    default:
                        break;
                }
            }

            var totalCount = await query.CountAsync();

            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            return new PaginatedListDto<ProductDto>(
                    items.Select(e => new ProductDto(e.Id, e.Name, e.Description, e.Price, e.CategoryId)),
                    new PaginationMeta(request.Page, request.PageSize, totalCount)
            );

        }

        public async Task<ProductDto> Create(CreateProductRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId) ?? throw new NullReferenceException("Category not exist!");
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Description = request.Description
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.CategoryId);
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _context.Products
            .FirstOrDefaultAsync(e => e.Id == id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            else throw new NullReferenceException("Product not found!");
        }

        public async Task<ProductDto> Get(int id)
        {
            var product = await _context.Products
                   .FirstOrDefaultAsync(e => e.Id == id);

            return product is not null
                ? new ProductDto(product.Id, product.Name, product.Description, product.Price, product.CategoryId)
                : throw new NullReferenceException("Product not found!");
        }
    }
}
