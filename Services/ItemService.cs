using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IItemService
    {
        Task<ServiceResult<List<ItemDTO>>> GetAllItemsAsync();
        Task<ServiceResult<ItemDTO>> GetItemByIdAsync(string itemId);
        Task<ServiceResult<ItemDTO>> CreateItemAsync(CreateItemDTO createDto);
        Task<ServiceResult<ItemDTO>> UpdateItemAsync(string itemId, UpdateItemDTO updateDto);
        Task<ServiceResult<bool>> DeleteItemAsync(string itemId);
    }
    public class ItemService : IItemService
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public async Task<ServiceResult<ItemDTO>> CreateItemAsync(CreateItemDTO createDto)
        {
            if (!Enum.TryParse<ItemType>(createDto.ItemType, true, out var itemTypeEnum))
            {
                return ServiceResult<ItemDTO>.Error($"El tipo de ítem '{createDto.ItemType}' no es válido.");
            }

            var newitem = new MItems
            {
                Name = createDto.Name,
                Description = createDto.Description,
                ItemType = itemTypeEnum,
                Value = createDto.Value,
                Stats = new ItemStats
                {
                    Health = createDto.Stats.Health,
                    Mana = createDto.Stats.Mana,
                    Strength = createDto.Stats.Strength,
                    Dexterity = createDto.Stats.Dexterity,
                    Intelligence = createDto.Stats.Intelligence,
                    Vitality = createDto.Stats.Vitality,
                    Armor = createDto.Stats.Armor,
                    MagicResist = createDto.Stats.MagicResist,
                    CriticalChance = createDto.Stats.CriticalChance,
                    AttackSpeed = createDto.Stats.AttackSpeed
                },
                CreatedBy = "Admin"
            };

            var createdItem = await _itemsRepository.CreateAsync(newitem);

            var response = new ItemDTO
            {
                Id = createdItem.Id,
                Name = createdItem.Name,
                Description = createdItem.Description,
                value = createdItem.Value,
                ItemType = createdItem.ItemType.ToString(),
                stats = createdItem.Stats.GetActiveStats()
            };

            return ServiceResult<ItemDTO>.Success(response);
        }

        public async Task<ServiceResult<bool>> DeleteItemAsync(string itemId)
        {
            var item = await _itemsRepository.GetByIdAsync(itemId);
            if(item == null)
            {
                return ServiceResult<bool>.Error("El item a borrar no existe");
            }

            await _itemsRepository.DeleteAsync(itemId);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<List<ItemDTO>>> GetAllItemsAsync()
        {
            var items = await _itemsRepository.GetAllAsync();

            if(items == null)
            {
                return ServiceResult<List<ItemDTO>>.NotFound("No hay items creados");
            }

            var itemsDto = items.Select(item => new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                value = item.Value,
                ItemType = item.ItemType.ToString(),
                stats = item.Stats.GetActiveStats()
            }).ToList();

            return ServiceResult<List<ItemDTO>>.Success(itemsDto);
        }

        public async Task<ServiceResult<ItemDTO>> GetItemByIdAsync(string itemId)
        {
            var itemDb = await _itemsRepository.GetByIdAsync(itemId);

            if(itemDb == null)
            {
                return ServiceResult<ItemDTO>.NotFound("El item no fue encontrado");
            }

            var response = new ItemDTO
            {
                Id = itemDb.Id,
                Name = itemDb.Name,
                Description = itemDb.Description,
                value = itemDb.Value,
                ItemType = itemDb.ItemType.ToString(),
                stats = itemDb.Stats.GetActiveStats()
            };

            return ServiceResult<ItemDTO>.Success(response);
        }

        public async Task<ServiceResult<ItemDTO>> UpdateItemAsync(string itemId, UpdateItemDTO updateDto)
        {
            var itemToUpdate = await _itemsRepository.GetByIdAsync(itemId);
            if( itemToUpdate == null)
            {
                return ServiceResult<ItemDTO>.NotFound("EL item que intentas actualizar no existe");
            }

            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                itemToUpdate.Name = updateDto.Name;
            }
            if (!string.IsNullOrEmpty(updateDto.Description))
            {
                itemToUpdate.Description = updateDto.Description;
            }
            if (updateDto.Value.HasValue)
            {
                itemToUpdate.Value = updateDto.Value.Value;
            }

            if (!string.IsNullOrEmpty(updateDto.ItemType))
            {
                if (Enum.TryParse<ItemType>(updateDto.ItemType, true, out var itemTypeEnum))
                {
                    itemToUpdate.ItemType = itemTypeEnum;
                }
                else
                {
                    return ServiceResult<ItemDTO>.Error($"El tipo de ítem '{updateDto.ItemType}' no es válido.");
                }
            }

            if (updateDto.Stats != null)
            {
                // Obtenemos las stats actuales del objeto
                var currentStats = itemToUpdate.Stats;

                // Actualizamos cada stat solo si se proporciona un nuevo valor en el DTO
                if (updateDto.Stats.Health.HasValue) currentStats.Health = updateDto.Stats.Health;
                if (updateDto.Stats.Mana.HasValue) currentStats.Mana = updateDto.Stats.Mana;
                if (updateDto.Stats.Strength.HasValue) currentStats.Strength = updateDto.Stats.Strength;
                if (updateDto.Stats.Dexterity.HasValue) currentStats.Dexterity = updateDto.Stats.Dexterity;
                if (updateDto.Stats.Intelligence.HasValue) currentStats.Intelligence = updateDto.Stats.Intelligence;
                if (updateDto.Stats.Vitality.HasValue) currentStats.Vitality = updateDto.Stats.Vitality;
                if (updateDto.Stats.Armor.HasValue) currentStats.Armor = updateDto.Stats.Armor;
                if (updateDto.Stats.MagicResist.HasValue) currentStats.MagicResist = updateDto.Stats.MagicResist;
                if (updateDto.Stats.CriticalChance.HasValue) currentStats.CriticalChance = updateDto.Stats.CriticalChance;
                if (updateDto.Stats.AttackSpeed.HasValue) currentStats.AttackSpeed = updateDto.Stats.AttackSpeed;

                // Reasignamos el objeto de stats modificado a la propiedad del item
                // para que se active la serialización a JSON
                itemToUpdate.Stats = currentStats;
            }

            await _itemsRepository.UpdateAsync(itemToUpdate);

            var response = new ItemDTO
            {
                Id = itemToUpdate.Id,
                Name = itemToUpdate.Name,
                Description = itemToUpdate.Description,
                value = itemToUpdate.Value,
                ItemType = itemToUpdate.ItemType.ToString(),
                stats = itemToUpdate.Stats.GetActiveStats()
            };

            return ServiceResult<ItemDTO>.Success(response);
        }
    }
}
