using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;
using Microsoft.AspNetCore.SignalR;
using DungeonCrawlerAPI.Hubs; // Asegúrate de crear este Hub

namespace DungeonCrawlerAPI.Services
{
    public interface IAuctionService
    {
        Task<ServiceResult<AuctionDTO>> CreateAuctionAsync(string characterId, CreateAuctionDTO auctionDto);
        Task<ServiceResult<List<AuctionDTO>>> GetActiveAuctionsAsync();
        Task<ServiceResult<BidDTO>> PlaceBidAsync(string auctionId, string characterId, CreateBidDTO bidDto);
    }

    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly IHubContext<AuctionHub> _hubContext;

        public AuctionService(
            IAuctionRepository auctionRepository,
            IBidRepository bidRepository,
            ICharacterRepository characterRepository,
            IItemsRepository itemsRepository,
            IHubContext<AuctionHub> hubContext)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _characterRepository = characterRepository;
            _itemsRepository = itemsRepository;
            _hubContext = hubContext;
        }

        public async Task<ServiceResult<AuctionDTO>> CreateAuctionAsync(string characterId, CreateAuctionDTO auctionDto)
        {
            // Lógica para crear una subasta
            var seller = await _characterRepository.GetByIdAsync(characterId);
            var item = await _itemsRepository.GetByIdAsync(auctionDto.ItemId);

            if (item == null || item.InventaryId == null) // Asumimos que si está en una subasta no está en el inventario
            {
                return ServiceResult<AuctionDTO>.Error("El ítem no es válido para subastar.");
            }

            var newAuction = new MAuction
            {
                ItemId = auctionDto.ItemId,
                SellerCharacterId = characterId,
                StartingPrice = auctionDto.StartingPrice,
                BuyoutPrice = auctionDto.BuyoutPrice,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(auctionDto.DurationInHours),
                AuctionStatus = AuctionStatus.Active,
                CreatedBy = seller.Name
            };

            var createdAuction = await _auctionRepository.CreateAsync(newAuction);

            // Notificar a los clientes a través de SignalR
            await _hubContext.Clients.All.SendAsync("NewAuction", createdAuction);

            var auctionDtoResult = new AuctionDTO { /* Mapear datos */ };
            return ServiceResult<AuctionDTO>.Success(auctionDtoResult);
        }

        public async Task<ServiceResult<List<AuctionDTO>>> GetActiveAuctionsAsync()
        {
            // Lógica para obtener subastas activas
            var auctions = await _auctionRepository.GetAllAsync(); // Deberías filtrar por activas
            var activeAuctionsDto = auctions.Select(a => new AuctionDTO { /* Mapear datos */ }).ToList();
            return ServiceResult<List<AuctionDTO>>.Success(activeAuctionsDto);
        }

        public async Task<ServiceResult<BidDTO>> PlaceBidAsync(string auctionId, string characterId, CreateBidDTO bidDto)
        {
            var auction = await _auctionRepository.GetByIdAsync(auctionId);
            var bidder = await _characterRepository.GetByIdAsync(characterId);

            if (auction == null || auction.AuctionStatus != AuctionStatus.Active)
            {
                return ServiceResult<BidDTO>.Error("La subasta no está activa.");
            }

            // Aquí deberías incluir la lógica de validación de la puja (ej. que sea mayor al precio actual)

            var newBid = new MBid
            {
                AuctionId = auctionId,
                BidderCharacter = characterId,
                Amount = bidDto.Amount,
                BidTime = DateTime.UtcNow,
                CreatedBy = bidder.Name
            };

            await _bidRepository.CreateAsync(newBid);

            // Notificar a los clientes sobre la nueva puja
            await _hubContext.Clients.Group(auctionId).SendAsync("NewBid", new { auctionId, bid = newBid });

            var bidDtoResult = new BidDTO { /* Mapear datos */ };
            return ServiceResult<BidDTO>.Success(bidDtoResult);
        }
    }
}