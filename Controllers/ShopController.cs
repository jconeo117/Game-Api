using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly ICharacterService _characterService;

        public ShopController(IShopService shopService, ICharacterService characterService)
        {
            _shopService = shopService;
            _characterService = characterService;
        }

        /// <summary>
        /// Obtiene todos los items disponibles en la tienda del sistema.
        /// GET: api/Shop/items
        /// </summary>
        [HttpGet("items")]
        public async Task<IActionResult> GetShopItems()
        {
            var result = await _shopService.GetShopItemsAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
            return Ok(result.Data);
        }

        /// <summary>
        /// Permite al personaje del jugador comprar un item.
        /// POST: api/Shop/buy/{itemId}
        /// </summary>
        [HttpPost("buy/{itemId}")]
        [Authorize]
        public async Task<IActionResult> BuyItem(string itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var characterResult = await _characterService.GetCharacterByUserId(userId);
            if (!characterResult.IsSuccess || characterResult.Data == null)
            {
                return BadRequest(new { message = "No se encontró un personaje para este usuario." });
            }

            var result = await _shopService.BuyItemAsync(characterResult.Data.Id, itemId);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = "¡Ítem comprado con éxito!" });
        }
    }
}
