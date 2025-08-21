using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IInventoryService
    {
        Task<ServiceResult<MInventory>> CreateNewInventoryAsync(string CharId);
        Task<ServiceResult<InventoryDTO>> GetInventoryById(string CharId);
        
    }
    public class InventoryService : IInventoryService
    {

        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemsRepository _itemsRepository;

        public InventoryService(IInventoryRepository inventoryRepository, IItemsRepository itemsRepository)
        {
            _inventoryRepository = inventoryRepository;
            _itemsRepository = itemsRepository;
        }

        public async Task<ServiceResult<MInventory>> CreateNewInventoryAsync(string CharId)
        {
            var NewInventory = new MInventory
            {
                UserId = CharId,
                CreatedBy = "System"
            };

            var CreatedInventory = await _inventoryRepository.CreateAsync(NewInventory);
            var StartItems = CreateInitialInventory(CreatedInventory.Id);

            foreach (var item in StartItems)
            {
                await _itemsRepository.CreateAsync(item);
            }

            return ServiceResult<MInventory>.Success(CreatedInventory);
        }

        public Task<ServiceResult<InventoryDTO>> GetInventoryById(string CharId)
        {
            throw new NotImplementedException();
        }

        private List<MItems> CreateInitialInventory(string InventoryId)
        {
            return new List<MItems>
            {
                new MItems
                {
                    Name = "Espada de cobre",
                    Description = "Espada de cobre para novatos",
                    ItemType = ItemType.Weapon,
                    Value = 1,
                    InventaryId = InventoryId,
                    Stats = new ItemStats{Strength = 3},
                    CreatedBy = "System"
                },
                new MItems
                {
                    Name = "Cota de malla",
                    Description = "Cota de malla para novatos",
                    ItemType = ItemType.Armor,
                    Value = 3,
                    InventaryId = InventoryId,
                    Stats = new ItemStats{Armor = 2, Vitality = 1},
                    CreatedBy = "System"
                },
                new MItems
                {
                    Name = "pocion de salud pequeña",
                    Description = "Pocion de recuperacion de vida pequeña",
                    ItemType = ItemType.Consumible,
                    Value = 3,
                    InventaryId = InventoryId,
                    Stats = new ItemStats{Health = 10},
                    CreatedBy = "System"
                },

            };
        }
    }
}
