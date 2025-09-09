using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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


    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DungeonRunController : ControllerBase
    {
        private readonly IDungeonRunService _dungeonRunService;
        private readonly ICharacterService _characterService;
        public DungeonRunController(IDungeonRunService dungeonRunService, ICharacterService characterService)
        {
            _dungeonRunService = dungeonRunService;
            _characterService = characterService;
        }

        [HttpPost("{dungeonId}/start")]
        public async Task<IActionResult> StartDungeonRun(string dungeonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (userId == null)
            {
                return Unauthorized();
            }
            
            var UserChar = await _characterService.GetCharacterByUserId(userId);

            if(!UserChar.IsSuccess)
            {
                return BadRequest(UserChar?.ErrorMessage);
            }

            var response = await _dungeonRunService.StartDungeonRunAsync(UserChar?.Data?.Id, dungeonId);

            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpPost("{runId}/complete")]
        public async Task<IActionResult> CompleteDungeonRun(string runId, [FromBody] CompleteDungeonRunDTO completeDungeonRunDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (userId == null)
            {
                return Unauthorized();
            }

            var UserChar = await _characterService.GetCharacterByUserId(userId);
            if (!UserChar.IsSuccess)
            {
                return BadRequest(UserChar?.ErrorMessage);
            }

            var response = await _dungeonRunService.CompleteDungeonRunAsync(UserChar.Data.Id,runId, completeDungeonRunDTO);

            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }

        [HttpGet("my-runs")]
        public async Task<IActionResult> GetAllMyRuns()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (userId == null)
            {
                return Unauthorized();
            }

            var UserChar = await _characterService.GetCharacterByUserId(userId);
            if (!UserChar.IsSuccess)
            {
                return BadRequest(UserChar?.ErrorMessage);
            }

            var response = await _dungeonRunService.GetDungeonRunByCharacterAsync(UserChar.Data.Id);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);

        }
    }
}
