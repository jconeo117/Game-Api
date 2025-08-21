using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IInventoryRepository : IRepository<MInventory>
    {

    }
    public class InventoryRepository : Repository<MInventory>, IInventoryRepository
    {
        public InventoryRepository(AppDBContext context) : base(context)
        {
        }
    }
}
