using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IBidRepository : IRepository<MBid>
    {

    }
    public class BidRepository : Repository<MBid>, IBidRepository
    {
        public BidRepository(AppDBContext context) : base(context)
        {
        }
    }
}
