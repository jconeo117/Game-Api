using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IInventoryService
    {
        Task<ServiceResult<MInventory>> CreateNewInventoryAsync(string CharId);
        Task<ServiceResult<InventoryDTO>> GetInventoryById(string CharId);
        Task<ServiceResult<bool>> EquipItem(string characterId, string ItemId, EquipmentSlotType slotType);
        Task<ServiceResult<bool>> UnEquipItem(string characterId, EquipmentSlotType SlotType);

        
    }
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemsRepository _itemsRepository;
        private readonly IEquipmentSlotRepository _equipmentSlotRepository;
        public InventoryService(IInventoryRepository inventoryRepository, IItemsRepository itemsRepository, ICharacterRepository characterRepository, IEquipmentSlotRepository equipmentSlotRepository)
        {
            _inventoryRepository = inventoryRepository;
            _itemsRepository = itemsRepository;
            _equipmentSlotRepository = equipmentSlotRepository;
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

            var equipmentSlots = CreateEquipmentSlot(CharId);
            foreach (var slot in equipmentSlots)
            {
                await _equipmentSlotRepository.CreateAsync(slot);
            }

            return ServiceResult<MInventory>.Success(CreatedInventory);
        }

        public async Task<ServiceResult<bool>> EquipItem(string characterId, string itemId, EquipmentSlotType slotType)
        {
           var inventory = await _inventoryRepository.GetInventoryByCharId(characterId);
           var itemToEquip = inventory?.Items.FirstOrDefault(i => i.Id == itemId);

            if (itemToEquip == null)
            {
                return ServiceResult<bool>.NotFound("El Item no existe en el inventario");
            }

            var Slot = await _equipmentSlotRepository.GetSlotByCharacterAndTypeAsync(characterId, slotType);

            if (!string.IsNullOrEmpty(Slot.ItemId))
            {
                var OldItemId = Slot.ItemId;
                var OldItem = await _itemsRepository.GetByIdAsync(OldItemId);
                if(OldItem != null)
                {
                    OldItem.InventaryId = inventory?.Id;
                    await _itemsRepository.UpdateAsync(OldItem);
                }
            }

            itemToEquip.InventaryId = null;
            await _itemsRepository.UpdateAsync(itemToEquip);

            // Actualizar el slot si ya existe pero está vacío
            Slot.ItemId = itemId;
            await _equipmentSlotRepository.UpdateAsync(Slot);

            return ServiceResult<bool>.Success(true);
            
        }

        public async Task<ServiceResult<InventoryDTO>> GetInventoryById(string CharId)
        {
            var inventory =  await _inventoryRepository.GetInventoryByCharId(CharId);

            if(inventory == null)
            {
                return ServiceResult<InventoryDTO>.Error("El character no tiene inventario o no existe");
            }

            var response = new InventoryDTO
            {
                Id = inventory.Id,
                NumSlots = inventory.NumSlots,
                Items = inventory.Items.Select(item => new ItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    ItemType = item.ItemType.ToString(),
                    value = item.Value,
                    stats = item.Stats.GetActiveStats()
                }).ToList()
            };

            return ServiceResult<InventoryDTO>.Success(response);
        }

        public async Task<ServiceResult<bool>> UnEquipItem(string characterId, EquipmentSlotType SlotType)
        {
            var inventory = await _inventoryRepository.GetInventoryByCharId(characterId);
            if(inventory == null)
            {
                return ServiceResult<bool>.NotFound("El inventario no fue encontrado.");
            }

            var slot = await _equipmentSlotRepository.GetSlotByCharacterAndTypeAsync(characterId, SlotType);
            if(slot == null || string.IsNullOrEmpty(slot.ItemId))
            {
                return ServiceResult<bool>.NotFound("No hay item equipado en ese slot");
            }

            var itemToUnequip = await _itemsRepository.GetByIdAsync(slot.ItemId);
            if(itemToUnequip == null)
            {
                return ServiceResult<bool>.NotFound("El item a desequipar no fue encontrado");
            }

            itemToUnequip.InventaryId = inventory.Id;
            await _itemsRepository.UpdateAsync(itemToUnequip);

            slot.ItemId = null;
            await _equipmentSlotRepository.UpdateAsync(slot);

            return ServiceResult<bool>.Success(true);
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

        private List<MEquipmentSlot> CreateEquipmentSlot(string CharId)
        {
            List<MEquipmentSlot> equipmentSlots = new List<MEquipmentSlot>();

            foreach (EquipmentSlotType slotType in Enum.GetValues(typeof(EquipmentSlotType)))
            {
                var newSlot = new MEquipmentSlot
                {
                    CharacterId = CharId,
                    SlotType = slotType,
                    ItemId = null, // El slot se crea vacío
                    CreatedBy = "System"
                };
                equipmentSlots.Add(newSlot);
            }

            return equipmentSlots;
        }
    }
}
