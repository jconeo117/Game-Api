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
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterDTO characterDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if(userId == null)
            {
                return Unauthorized();
            }

            var response = await _characterService.CreateCharacterAsync(userId, characterDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet("character")]
        public async Task<IActionResult> GetCharacter()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if(userId == null)
            {
                return Unauthorized();
            }

            var response = await _characterService.GetCharacterByUserId(userId);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet("character/{CharId}")]
        public async Task<IActionResult> GetCharacterByCharId(string CharId)
        {
            var response = await _characterService.GetCharacterById(CharId);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet("character/{CharId}/profile")]
        public async Task<IActionResult> GetCharacterProfile(string CharId)
        {
            var response = await _characterService.GetCharacterProfileById(CharId);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

    }
}
