using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Cybersoft_store.Api.Models;

namespace Cybersoft_store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {

        private readonly IUserService _userService;
        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTModels()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            var res = await _userService.GetProfile(token);
            return StatusCode(res.StatusCode, res);
        }
    }
}