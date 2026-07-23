using Microsoft.AspNetCore.Mvc;

namespace Cybersoft_store.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(string? keyword, int page = 1, int pageSize = 10)
        {
            var response = await _productService.GetAll(keyword, page, pageSize);
            return StatusCode(response.StatusCode, response);
        }
    }
}