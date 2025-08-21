using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IItemsRepository : IRepository<MItems>
    {

    }
    public class ItemsRepository : Repository<MItems>, IItemsRepository
    {
        public ItemsRepository(AppDBContext context) : base(context)
        {
        }
    }
}
