using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ICharacterService _characterService;
        public InventoryController(IInventoryService inventoryService, ICharacterService characterService)
        {
            _inventoryService = inventoryService;
            _characterService = characterService;
        }

        [HttpGet("my-inventory")]
        public async Task<IActionResult> GetMyInventory()
        {
            // 1. Obtener el ID del usuario desde el token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (userId == null)
            {
                return Unauthorized();
            }

            // 2. Encontrar el personaje asociado a ese usuario
            var characterResult = await _characterService.GetCharacterByUserId(userId);
            if (!characterResult.IsSuccess)
            {
                return NotFound(new { message = "No se encontró un personaje para este usuario." });
            }

            // 3. Obtener el inventario usando el ID del personaje
            var inventoryResponse = await _inventoryService.GetInventoryById(characterResult.Data.Id);

            if (!inventoryResponse.IsSuccess)
            {
                return BadRequest(new { message = inventoryResponse.ErrorMessage });
            }

            return Ok(inventoryResponse.Data);
        }

        // Podrías mantener este endpoint si los Admins pueden ver cualquier inventario
        [HttpGet("character/{characterId}")]
        [Authorize(Roles = "Admin")] // Solo los admins pueden usar esta ruta
        public async Task<IActionResult> GetInventoryForAdmin(string characterId)
        {
            var response = await _inventoryService.GetInventoryById(characterId);

            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
    }
}
