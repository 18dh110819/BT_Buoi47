using Microsoft.AspNetCore.Mvc;

namespace Cybersoft_store.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			return Ok();
		}

		/// <summary>
		/// Đăng ký người dùng mới
		/// </summary>
		/// <param name="registerDto">Các thông tin cần thiết cho đăng ký bên phía user web(Không phải admin)</param>
		/// <returns></returns> <summary>
		/// 
		/// </summary>
		/// <param name="registerDto">Các thông tin cần thiết cho đăng ký bên phía user web(Không phải admin)</param>
		/// <returns></returns>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
		{
			var response = await _userService.Register(registerDto);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var res = await _userService.Login(loginDto);
			return StatusCode(res.StatusCode, res);
		}
	}
}
