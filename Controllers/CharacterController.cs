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
        private readonly IInventoryService _inventoryService;

        public CharacterController(ICharacterService characterService, IInventoryService inventoryService)
        {
            _characterService = characterService;
            _inventoryService = inventoryService;
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

        [HttpGet("my-character")]
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

        [HttpGet("{CharId}")]
        public async Task<IActionResult> GetCharacterByCharId(string CharId)
        {
            var response = await _characterService.GetCharacterById(CharId);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet("{CharId}/profile")]
        public async Task<IActionResult> GetCharacterProfile(string CharId)
        {
            var response = await _characterService.GetCharacterProfileById(CharId);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpPut("{charId}/equipment/equip")]
        public async Task<IActionResult> EquipItem(string charId, [FromBody] EquipItemRequestDTO itemRequestDTO)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return Unauthorized();
            }

            var userCharResult = await _characterService.GetCharacterByUserId(loggedInUserId);
            if (!userCharResult.IsSuccess)
            {
                return NotFound(new { Message = userCharResult.ErrorMessage });
            }

            if (userCharResult.Data.Id != charId)
            {
                return Forbid();
            }

            var result = await _inventoryService.EquipItem(charId, itemRequestDTO.ItemId, itemRequestDTO.SlotType);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return Ok(new { message = "Ítem equipado correctamente." });
        }

        [HttpDelete("{charId}/equipment/unequip")]
        public async Task<IActionResult> UnequipItem(string charId,[FromBody] UnequipItemRequestDTO itemRequestDTO)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return Unauthorized();
            }

            var userCharResult = await _characterService.GetCharacterByUserId(loggedInUserId);
            if (!userCharResult.IsSuccess)
            {
                return NotFound(new { Message = userCharResult.ErrorMessage });
            }

            if (userCharResult.Data.Id != charId)
            {
                return Forbid();
            }

            var result = await _inventoryService.UnEquipItem(charId, itemRequestDTO.SlotType);
            if (!result.IsSuccess)
            {
                return BadRequest(new {Message =  result.ErrorMessage});
            }

            return Ok(new { message = "Ítem desequipado correctamente." });
        }


    }
}
