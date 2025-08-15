using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController:ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await _authService.GetProfileAsync(userId);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        [HttpPost("update-profile")]
        public async Task <IActionResult> UpdateProfile([FromBody] UserDTO userDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if(userId == null)
            {
                return Unauthorized();
            }

            var result = await _authService.UpdateProfileAsync(userId, userDTO);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changepasswordDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _authService.ChangePasswordAsync(userId, changepasswordDTO);

            if (!result.IsSuccess)
            {
                return NotFound(new {message = result.ErrorMessage});
            }

            return Ok(result.Data);
        }
    }
}
