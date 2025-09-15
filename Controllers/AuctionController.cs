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
    public class AuctionController:ControllerBase
    {

        private readonly IAuctionService _auctionService;
        private readonly ICharacterService _characterService;

        public AuctionController(IAuctionService auctionService, ICharacterService characterService)
        {
            _auctionService = auctionService;
            _characterService = characterService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAuctions()
        {
            var result = await _auctionService.GetActiveAuctionsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDTO auctionDto)
        {
            // Obtener el ID del usuario a partir del token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Obtener el personaje asociado al usuario
            var characterResult = await _characterService.GetCharacterByUserId(userId);
            if (!characterResult.IsSuccess)
            {
                return BadRequest(new { message = "No se encontró un personaje para este usuario." });
            }

            // Llamar al servicio para crear la subasta
            var result = await _auctionService.CreateAuctionAsync(characterResult.Data.Id, auctionDto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

       
        [HttpPost("{auctionId}/bid")]
        public async Task<IActionResult> PlaceBid(string auctionId, [FromBody] CreateBidDTO bidDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var characterResult = await _characterService.GetCharacterByUserId(userId);
            if (!characterResult.IsSuccess)
            {
                return BadRequest(new { message = "No se encontró un personaje para este usuario." });
            }

            // Llamar al servicio para realizar la puja
            var result = await _auctionService.PlaceBidAsync(auctionId, characterResult.Data.Id, bidDto);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }
    }

}
