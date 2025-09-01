using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles = "Admin")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllItems()
        {
            var response = await _itemService.GetAllItemsAsync();

            if (!response.IsSuccess)
            {
                return BadRequest(new {Message = response.ErrorMessage});
            }

            return Ok(response.Data);
        }

        [HttpGet("{ItemId}")]
        public async Task<IActionResult> GetItemById(string ItemId)
        {
            var response = await _itemService.GetItemByIdAsync(ItemId);

            if (!response.IsSuccess)
            {
                return NotFound(new {Message = response.ErrorMessage });
            }

            return Ok(response.Data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDTO createItemDTO)
        {
            var response = await _itemService.CreateItemAsync(createItemDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(new { Message = response.ErrorMessage});
            }

            return Ok(response.Data);
        }

        [HttpPut("update/{ItemId}")]
        public async Task<IActionResult> UpdateItem(string ItemId, [FromBody]UpdateItemDTO updateItemDTO)
        {
            var response = await _itemService.UpdateItemAsync(ItemId, updateItemDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(new {Message = response.ErrorMessage});
            }

            return Ok(response.Data);
        }

        [HttpDelete("delete/{ItemId}")]
        public async Task<IActionResult> DeleteItem(string ItemId)
        {
            var response = await _itemService.DeleteItemAsync(ItemId);

            if (!response.IsSuccess)
            {
                return NotFound(new {Message = response.ErrorMessage});
            }

            return Ok(response.Data);
        }
    }
}
