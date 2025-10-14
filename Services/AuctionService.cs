using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Hubs; // Asegúrate de crear este Hub
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace DungeonCrawlerAPI.Services
{
    public interface IAuctionService
    {
        Task<ServiceResult<AuctionDTO>> CreateAuctionAsync(string characterId, CreateAuctionDTO auctionDto);
        Task<ServiceResult<List<AuctionDTO>>> GetActiveAuctionsAsync(string? itemName = null, decimal? minPrice = null, decimal? maxPrice = null, string? itemType = null);
        Task<ServiceResult<BidDTO>> PlaceBidAsync(string auctionId, string characterId, CreateBidDTO bidDto);
        Task ProcessExpiredAuctionsAsync();
    }

    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly IHubContext<AuctionHub> _hubContext;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<AuctionService> _logger;
        private readonly AppDBContext _context;

        public AuctionService(
            IAuctionRepository auctionRepository,
            IBidRepository bidRepository,
            ICharacterRepository characterRepository,
            IItemsRepository itemsRepository,
            IHubContext<AuctionHub> hubContext,
            IInventoryRepository inventoryRepository,
            ILogger<AuctionService> logger,
            AppDBContext context
            )
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _characterRepository = characterRepository;
            _itemsRepository = itemsRepository;
            _hubContext = hubContext;
            _inventoryRepository = inventoryRepository;
            _logger = logger;
            _context = context;
        }

        public async Task<ServiceResult<AuctionDTO>> CreateAuctionAsync(string characterId, CreateAuctionDTO auctionDto)
        {
            var seller = await _characterRepository.GetByIdAsync(characterId);
            if (seller == null) return ServiceResult<AuctionDTO>.NotFound("Vendedor no encontrado.");

            var inventory = await _inventoryRepository.GetInventoryByCharId(characterId);
            if (inventory == null) return ServiceResult<AuctionDTO>.Error("Inventario no encontrado.");

            // El ítem debe existir Y pertenecer al inventario del vendedor
            var item = await _itemsRepository.GetByIdAsync(auctionDto.ItemId);
            if (item == null || item.InventaryId != inventory.Id)
            {
                return ServiceResult<AuctionDTO>.Error("El ítem no se encuentra en tu inventario.");
            }

            // Quitar el ítem del inventario para ponerlo en "escrow"
            item.InventaryId = null;
            await _itemsRepository.UpdateAsync(item);

            var newAuction = new MAuction
            {
                ItemId = auctionDto.ItemId,
                SellerCharacterId = characterId,
                StartingPrice = auctionDto.StartingPrice,
                BuyoutPrice = auctionDto.BuyoutPrice,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(auctionDto.DurationInHours),
                AuctionStatus = AuctionStatus.Active,
                CreatedBy = seller.Name
            };

            var createdAuction = await _auctionRepository.CreateAsync(newAuction);

            // Notificar a los clientes a través de SignalR
            await _hubContext.Clients.All.SendAsync("NewAuction", createdAuction);

            var auctionDtoResult = new AuctionDTO
            {
                Id = createdAuction.Id,
                ItemId = item.Id,
                Item = new ItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    ItemType = item.ItemType.ToString(),
                    value = item.Value,
                    stats = item.Stats.GetActiveStats()
                },
                SellerCharacterId = characterId,
                SellerCharacterName = seller.Name,
                StartingPrice = createdAuction.StartingPrice,
                BuyoutPrice = createdAuction.BuyoutPrice,
                CurrentPrice = createdAuction.StartingPrice,
                EndTime = createdAuction.EndTime,
                Status = createdAuction.AuctionStatus.ToString(),
            };

            return ServiceResult<AuctionDTO>.Success(auctionDtoResult);
        }

        public async Task<ServiceResult<List<AuctionDTO>>> GetActiveAuctionsAsync(string? itemName = null,
                                                                                decimal? minPrice = null,
                                                                                decimal? maxPrice = null,
                                                                                string? itemType = null)
        {
            // Obtener todas las subastas activas desde el repositorio
            var auctions = await _auctionRepository.GetActiveAuctionsWithDetailsAsync();

            // Aplicar filtros ANTES del mapeo a DTO
            var filteredAuctions = auctions.AsEnumerable();

            if (!string.IsNullOrEmpty(itemName))
            {
                filteredAuctions = filteredAuctions.Where(a =>
                    a.mItem.Name.Contains(itemName, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                filteredAuctions = filteredAuctions.Where(a =>
                {
                    var currentPrice = a.Bids.Any() ? a.Bids.Max(b => b.Amount) : a.StartingPrice;
                    return currentPrice >= minPrice.Value;
                });
            }

            if (maxPrice.HasValue)
            {
                filteredAuctions = filteredAuctions.Where(a =>
                {
                    var currentPrice = a.Bids.Any() ? a.Bids.Max(b => b.Amount) : a.StartingPrice;
                    return currentPrice <= maxPrice.Value;
                });
            }

            if (!string.IsNullOrEmpty(itemType))
            {
                filteredAuctions = filteredAuctions.Where(a =>
                    a.mItem.ItemType.ToString().Equals(itemType, StringComparison.OrdinalIgnoreCase));
            }

            // AHORA SÍ mapear a DTO después de filtrar
            var activeAuctionsDto = filteredAuctions.Select(a => new AuctionDTO
            {
                Id = a.Id,
                ItemId = a.ItemId,
                SellerCharacterId = a.SellerCharacterId,
                SellerCharacterName = a.character.Name,
                StartingPrice = a.StartingPrice,
                BuyoutPrice = a.BuyoutPrice,
                CurrentPrice = a.Bids.Any() ? a.Bids.Max(b => b.Amount) : a.StartingPrice,
                EndTime = a.EndTime,
                Status = a.AuctionStatus.ToString(),
                Item = new ItemDTO
                {
                    Id = a.mItem.Id,
                    Name = a.mItem.Name,
                    Description = a.mItem.Description,
                    ItemType = a.mItem.ItemType.ToString(),
                    value = a.mItem.Value,
                    stats = a.mItem.Stats.GetActiveStats()
                },
                Bids = a.Bids.OrderByDescending(b => b.BidTime).Select(b => new BidDTO
                {
                    BidderCharacterName = b.character.Name,
                    Amount = b.Amount,
                    BidTime = b.BidTime
                }).ToList()
            }).ToList();

            return ServiceResult<List<AuctionDTO>>.Success(activeAuctionsDto);
        }

        public async Task<ServiceResult<BidDTO>> PlaceBidAsync(string auctionId, string characterId, CreateBidDTO bidDto)
        {
            // Inicia la transacción. Todo dentro del 'using' es parte de la "caja mágica".
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var auction = await _auctionRepository.GetByIdAsync(auctionId);
                    var bidder = await _characterRepository.GetByIdAsync(characterId);

                    if (auction == null || auction.AuctionStatus != AuctionStatus.Active)
                    {
                        return ServiceResult<BidDTO>.Error("La subasta no está activa.");
                    }

                    if (bidder == null)
                    {
                        return ServiceResult<BidDTO>.Error("El personaje no existe.");
                    }

                    if (auction.SellerCharacterId == characterId)
                    {
                        return ServiceResult<BidDTO>.Error("No puedes pujar en tu propia subasta.");
                    }

                    var highestBid = await _bidRepository.GetHighestBidForAuctionAsync(auctionId);
                    var currentHighestAmount = highestBid?.Amount ?? auction.StartingPrice;

                    if (bidDto.Amount <= currentHighestAmount)
                    {
                        return ServiceResult<BidDTO>.Error($"La puja debe ser mayor a {currentHighestAmount:C}");
                    }

                    if (bidder.Gold < bidDto.Amount)
                    {
                        return ServiceResult<BidDTO>.Error("No tienes suficiente dinero para realizar esta puja.");
                    }

                    // Si había otra puja más alta de OTRO jugador, devolverle el dinero.
                    if (highestBid != null)
                    {
                        var previousHighestBidder = await _characterRepository.GetByIdAsync(highestBid.BidderCharacter);
                        if (previousHighestBidder != null)
                        {
                            previousHighestBidder.Gold += (int)highestBid.Amount;
                            _context.Characters.Update(previousHighestBidder); // Preparamos el cambio
                        }
                    }

                    // Descontar el dinero de la nueva puja
                    bidder.Gold -= (int)bidDto.Amount;
                    _context.Characters.Update(bidder); // Preparamos el cambio

                    var newBid = new MBid
                    {
                        AuctionId = auctionId,
                        BidderCharacter = characterId,
                        Amount = bidDto.Amount,
                        BidTime = DateTime.UtcNow,
                        CreatedBy = bidder.Name
                    };

                    await _context.Bids.AddAsync(newBid); // Preparamos la nueva puja

                    // ¡Solo guardamos todo en la base de datos UNA VEZ!
                    await _context.SaveChangesAsync();

                    // Si llegamos aquí sin errores, confirmamos todos los cambios.
                    await transaction.CommitAsync();

                    // La notificación de SignalR va DESPUÉS de confirmar la transacción.
                    await _hubContext.Clients.Group(auctionId).SendAsync("NewBid", new
                    {
                        auctionId = auctionId,
                        bidderName = bidder.Name,
                        amount = newBid.Amount,
                        bidTime = newBid.BidTime
                    });

                    var bidDtoResult = new BidDTO { /* Mapear tus datos */ };
                    return ServiceResult<BidDTO>.Success(bidDtoResult);
                }
                catch (Exception ex)
                {
                    // Si algo falló, revertimos TODO.
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error al procesar la puja, la transacción fue revertida.");
                    return ServiceResult<BidDTO>.Error("Ocurrió un error al procesar tu puja.");
                }
            }
        }

        public async Task ProcessExpiredAuctionsAsync()
        {
            // 1. Encontrar todas las subastas activas cuya fecha de fin ya pasó
            var expiredAuctions = await _auctionRepository.GetAllAsync(
                a => a.AuctionStatus == AuctionStatus.Active && a.EndTime <= DateTime.UtcNow
            );

            foreach (var auction in expiredAuctions)
            {
                var bids = await _bidRepository.GetAllAsync(b => b.AuctionId == auction.Id);
                var winningBid = bids.OrderByDescending(b => b.Amount).FirstOrDefault();

                if (winningBid != null)
                {
                    // -- Subasta VENDIDA --
                    auction.AuctionStatus = AuctionStatus.Sold;
                    var winner = await _characterRepository.GetByIdAsync(winningBid.BidderCharacter);
                    var seller = await _characterRepository.GetByIdAsync(auction.SellerCharacterId);
                    var item = await _itemsRepository.GetByIdAsync(auction.ItemId);
                    var winnerInventory = await _inventoryRepository.GetInventoryByCharId(winner.Id);

                    // Transferir ítem al ganador
                    item.InventaryId = winnerInventory.Id;
                    await _itemsRepository.UpdateAsync(item);

                    // Transferir oro al vendedor (considera una comisión)
                    seller.Gold += (int)winningBid.Amount;
                    await _characterRepository.UpdateAsync(seller);

                    _logger.LogInformation($"Subasta {auction.Id} vendida a {winner.Name} por {winningBid.Amount}.");
                    await _hubContext.Clients.Group(auction.Id).SendAsync("AuctionEnded", new
                    {
                        auctionId = auction.Id,
                        status = "Vendido",
                        winnerName = winner.Name,
                        finalPrice = winningBid.Amount
                    });
                }
                else
                {
                    // -- Subasta EXPIRADA (sin pujas) --
                    auction.AuctionStatus = AuctionStatus.Expired;
                    var sellerInventory = await _inventoryRepository.GetInventoryByCharId(auction.SellerCharacterId);
                    var item = await _itemsRepository.GetByIdAsync(auction.ItemId);

                    // Devolver el ítem al vendedor
                    item.InventaryId = sellerInventory.Id;
                    await _itemsRepository.UpdateAsync(item);

                    _logger.LogInformation($"Subasta {auction.Id} expiró sin pujas.");
                    await _hubContext.Clients.Group(auction.Id).SendAsync("AuctionEnded", new
                    {
                        auctionId = auction.Id,
                        status = "Expirado",
                        winnerName = (string)null,
                        finalPrice = 0
                    });
                }

                await _auctionRepository.UpdateAsync(auction);
            }
        }
    }
}