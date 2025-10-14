using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{
    public interface IShopService
    {
        Task<ServiceResult<List<ItemDTO>>> GetShopItemsAsync();
        Task<ServiceResult<bool>> BuyItemAsync(string characterId, string itemId);
    }

    public class ShopService : IShopService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ShopService(IItemsRepository itemsRepository, ICharacterRepository characterRepository, IInventoryRepository inventoryRepository)
        {
            _itemsRepository = itemsRepository;
            _characterRepository = characterRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ServiceResult<List<ItemDTO>>> GetShopItemsAsync()
        {
            // AHORA SOLO TRAEMOS LOS ITEMS MARCADOS PARA LA TIENDA DEL SISTEMA
            var items = await _itemsRepository.GetAllAsync(item => item.IsSystemItem);

            var itemsDto = items.Select(item => new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ItemType = item.ItemType.ToString(),
                value = item.Value,
                stats = item.Stats.GetActiveStats()
            }).ToList();

            return ServiceResult<List<ItemDTO>>.Success(itemsDto);
        }

        public async Task<ServiceResult<bool>> BuyItemAsync(string characterId, string itemId)
        {
            var character = await _characterRepository.GetByIdAsync(characterId);
            if (character == null)
            {
                return ServiceResult<bool>.NotFound("Personaje no encontrado.");
            }

            // El item que se busca es la "plantilla" del sistema
            var itemTemplate = await _itemsRepository.GetByIdAsync(itemId);
            if (itemTemplate == null || !itemTemplate.IsSystemItem)
            {
                return ServiceResult<bool>.NotFound("Ítem no encontrado en la tienda.");
            }

            if (character.Gold < itemTemplate.Value)
            {
                return ServiceResult<bool>.Error("No tienes suficiente oro.");
            }

            var characterInventory = await _inventoryRepository.GetInventoryByCharId(characterId);
            if (characterInventory == null)
            {
                return ServiceResult<bool>.Error("El personaje no tiene inventario.");
            }

            // Deducir el oro
            character.Gold -= itemTemplate.Value;
            await _characterRepository.UpdateAsync(character);

            // Crear una nueva instancia del ítem para el inventario del jugador
            var newItemForPlayer = new MItems
            {
                Name = itemTemplate.Name,
                Description = itemTemplate.Description,
                ItemType = itemTemplate.ItemType,
                Value = itemTemplate.Value,
                Stats = itemTemplate.Stats,
                IsSystemItem = false, // La copia que va al jugador no es un ítem de sistema
                InventaryId = characterInventory.Id, // <- Asociamos al inventario del personaje
                CreatedBy = "ShopService"
            };

            await _itemsRepository.CreateAsync(newItemForPlayer);

            return ServiceResult<bool>.Success(true);
        }
    }
}
