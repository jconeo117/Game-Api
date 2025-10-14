using DungeonCrawlerAPI.Data;
using DungeonCrawlerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Authorize(Roles ="Admin")]
    [Route("api/admin/shop")]
    public class AdminShopController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly ShopItemsSeeder _seeder;

        public AdminShopController(AppDBContext context)
        {
            _context = context;
            _seeder = new ShopItemsSeeder(context);
        }

        /// <summary>
        /// Seed inicial de items de la tienda
        /// POST: api/admin/shop/seed
        /// </summary>
        [HttpPost("seed")]
        public async Task<IActionResult> SeedShopItems()
        {
            try
            {
                await _seeder.SeedShopItemsAsync();
                return Ok(new { message = "Items de la tienda agregados exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Limpia todos los items de la tienda
        /// DELETE: api/admin/shop/clear
        /// </summary>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearShopItems()
        {
            try
            {
                await _seeder.ClearShopItemsAsync();
                return Ok(new { message = "Items de la tienda eliminados exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Resetea la tienda (limpia y vuelve a poblar)
        /// POST: api/admin/shop/reset
        /// </summary>
        [HttpPost("reset")]
        public async Task<IActionResult> ResetShop()
        {
            try
            {
                await _seeder.ClearShopItemsAsync();
                await _seeder.SeedShopItemsAsync();
                return Ok(new { message = "Tienda reseteada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Agrega un item personalizado a la tienda
        /// POST: api/admin/shop/add-item
        /// </summary>
        [HttpPost("add-item")]
        public async Task<IActionResult> AddCustomItem([FromBody] AddShopItemRequest request)
        {
            try
            {
                await _seeder.AddShopItemAsync(
                    request.Name,
                    request.Description,
                    request.ItemType,
                    request.Value,
                    request.Stats
                );
                return Ok(new { message = $"Item '{request.Name}' agregado a la tienda" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class AddShopItemRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType ItemType { get; set; }
        public int Value { get; set; }
        public ItemStats Stats { get; set; }
    }
}