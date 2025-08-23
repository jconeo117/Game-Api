using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IInventoryRepository : IRepository<MInventory>
    {
        Task<MInventory?> GetInventoryByCharId(string  charId);
    }
    public class InventoryRepository : Repository<MInventory>, IInventoryRepository
    {
        public InventoryRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MInventory?> GetInventoryByCharId(string charId)
        {
            return await _dbset.Include(iv => iv.Items).FirstOrDefaultAsync(i => i.UserId == charId);
        }
    }
}
