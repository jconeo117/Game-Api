using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IAuctionRepository : IRepository<MAuction>
    {

    }

    public class AuctionRepository : Repository<MAuction>, IAuctionRepository
    {
        public AuctionRepository(AppDBContext context) : base(context)
        {
        }
    }
}
