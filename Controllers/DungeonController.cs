using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonCrawlerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class DungeonController : ControllerBase
    {
        private readonly IDungeonService _dungeonService;

        public DungeonController(IDungeonService dungeonService)
        {
            _dungeonService = dungeonService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDungeon (CreateDungeonDTO createDungeonDTO)
        {
            var response = await _dungeonService.CreateDungeon(createDungeonDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(new {Message = response.ErrorMessage});
            }

            return Ok(response);
        }

        [HttpPut("update/{DungeonId}")]
        public async Task<IActionResult> UpdateDungeon (string DungeonId, UpdateDungeonDTO createDungeonDTO)
        {
            var response = await _dungeonService.UpdateDungeon(DungeonId ,createDungeonDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(new {Message = response.ErrorMessage});
            }

            return Ok(response);
        }

        [HttpDelete("delete/{DungeonId}")]
        public async Task<IActionResult> DeleteDungeon (string DungeonId)
        {
            var response = await _dungeonService.DeleteDungeon(DungeonId);

            if (!response.IsSuccess)
            {
                return BadRequest(new {Message = response.ErrorMessage});
            }

            return Ok(response);
        }

    }
}
