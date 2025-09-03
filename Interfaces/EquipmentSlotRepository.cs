using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IEquipmentSlotRepository : IRepository<MEquipmentSlot>
    {
        Task<MEquipmentSlot?> GetSlotByCharacterAndTypeAsync(string characterId, EquipmentSlotType slotType);

    }

    public class EquipmentSlotRepository : Repository<MEquipmentSlot>, IEquipmentSlotRepository
    {
        public EquipmentSlotRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MEquipmentSlot?> GetSlotByCharacterAndTypeAsync(string characterId, EquipmentSlotType slotType)
        {
            return await _dbset.FirstOrDefaultAsync(s => s.CharacterId == characterId && s.SlotType == slotType);

        }
    }
}
