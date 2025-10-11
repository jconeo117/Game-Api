using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IAuctionRepository : IRepository<MAuction>
    {
        Task<IEnumerable<MAuction>> GetActiveAuctionsWithDetailsAsync();
    }

    public class AuctionRepository : Repository<MAuction>, IAuctionRepository
    {
        public AuctionRepository(AppDBContext context) : base(context)
        {
        }
        public async Task<IEnumerable<MAuction>> GetActiveAuctionsWithDetailsAsync()
        {
            // Usamos Include y ThenInclude para cargar los datos relacionados en una sola consulta
            return await _dbset
                .Include(a => a.mItem)        // Incluir la información del ítem
                .Include(a => a.character)    // Incluir la información del personaje vendedor
                .Include(a => a.Bids)         // Incluir las pujas
                    .ThenInclude(b => b.character) // E incluir el personaje de cada puja
                .Where(a => a.AuctionStatus == AuctionStatus.Active)
                .OrderBy(a => a.EndTime)
                .ToListAsync();
        }
    }
}
