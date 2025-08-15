using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerData)
        {
            var result = await _authService.RegisterAsync(registerData);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
        {
            var result = await _authService.LoginAsync(loginData);

            if (!result.IsSuccess)
            {
                return Unauthorized(new {message = result.ErrorMessage});
            }
            return Ok(result.Data);

        }
    }
}
