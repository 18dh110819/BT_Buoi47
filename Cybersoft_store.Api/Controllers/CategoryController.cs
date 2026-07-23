using Microsoft.AspNetCore.Mvc;
//using Cybersoft_store.Api.Models;

namespace Cybersoft_store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Lấy tất cả loại sản phẩm theo input người dùng
        /// </summary>
        /// <param name="keyword">Tìm kiếm theo input người dùng</param>
        /// <param name="page">Số trang hiện tại</param>
        /// <param name="pageSize">Số lượng hiển thị</param>
        /// <response code="200">Thành công lấy được dữ liệu.</response>
        /// <response code="400">Không lấy được dữ liệu. Lỗi từ hệ thống</response>
        /// <returns>Danh sách Loại sản phẩm</returns> <summary>
        /// 
        /// </summary>
        /// <param name="keyword">Tìm kiếm theo input người dùng</param>
        /// <param name="page">Số trang hiện tại</param>
        /// <param name="pageSize">Số lượng hiển thị</param>
        /// <response code="200">Thành công lấy được dữ liệu.</response>
        /// <response code="400">Không lấy được dữ liệu. Lỗi từ hệ thống</response>

        /// <returns>Danh sách Loại sản phẩm</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ResponseType<List<CategoryDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllCategory(string? keyword, int page = 1, int pageSize = 10)
        {
            var res = await _categoryService.GetAllCategoriesAsync(keyword, page, pageSize);
            return StatusCode(res.StatusCode, res);
        }


        /// <summary>
        /// Tạo mới loại sản phẩm cho cửa hàng
        /// </summary>
        /// <param name="dto">JSON để tạo loại sản phẩm</param>
        /// <returns>Thông báo việc tạo mới loại sản phẩm</returns> <summary>
        /// 
        /// <response code="201">Đã thêm loại sản phẩm trong cửa hàng.</response>
        /// <response code="409">Đã tồn tại loại sản phẩm trong cửa hàng</response>
        /// <response code="400">Đã xảy ra lỗi khi đang thêm loại sản phẩm vào cửa hàng</response>
        /// </summary>
        /// <param name="dto">JSON để tạo loại sản phẩm</param>
        /// <returns>Thông báo việc tạo mới loại sản phẩm</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ResponseType<string>), 201)]
        [ProducesResponseType(typeof(ResponseType<string>), 409)]
        [ProducesResponseType(typeof(ResponseType<string>), 400)]
        public async Task<IActionResult> GetAllCategory([FromBody] CategoryCreateDTO dto)
        {
            var res = await _categoryService.CreateCategoryAsync(dto);
            return StatusCode(res.StatusCode, res);
        }
    }
}