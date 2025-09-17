using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IBidRepository : IRepository<MBid>
    {
        Task<MBid?> GetHighestBidForAuctionAsync(string auctionId);
        Task<MBid?> GetLatestBidByUserAsync(string auctionId, string characterId);
        Task<List<MBid>> GetBidsByAuctionAsync(string auctionId);
    }

    public class BidRepository : Repository<MBid>, IBidRepository
    {
        public BidRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MBid?> GetHighestBidForAuctionAsync(string auctionId)
        {
            return await _context.Set<MBid>()
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Amount)
                .ThenByDescending(b => b.BidTime) // En caso de empate, la más reciente
                .FirstOrDefaultAsync();
        }

        public async Task<MBid?> GetLatestBidByUserAsync(string auctionId, string characterId)
        {
            return await _context.Set<MBid>()
                .Where(b => b.AuctionId == auctionId && b.BidderCharacter == characterId)
                .OrderByDescending(b => b.BidTime)
                .FirstOrDefaultAsync();
        }

        public async Task<List<MBid>> GetBidsByAuctionAsync(string auctionId)
        {
            return await _context.Set<MBid>()
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Amount)
                .ToListAsync();
        }
    }
}
