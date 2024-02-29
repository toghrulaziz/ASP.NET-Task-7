using ASP.NET_Task7.Models.DTOs.Pagination;
using ASP.NET_Task7.Models.DTOs.Product;
using ASP.NET_Task7.Providers;
using ASP.NET_Task7.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Task7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<TodoController> _logger;

        public ProductController(IProductService productService, ILogger<TodoController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        [HttpPost("create")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
        {
            try
            {
                return await _productService.Create(request);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new Product item");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request to create a new Product item");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                return await _productService.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Product item {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            try
            {
                var item = await _productService.Get(id);
                return item != null ? item : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting Product item {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("all")]
        public async Task<ActionResult<PaginatedListDto<ProductDto>>> All(PaginationRequest request, string? sortBy, int? categoryId, int? minPrice, int? maxPrice)
        {
            try
            {
                var result = await _productService.All(request, sortBy, categoryId, minPrice, maxPrice);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all Product items");
                return null;
            }
        }

    }
}
