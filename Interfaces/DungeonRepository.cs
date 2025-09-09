using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{
    public interface IDungeonRepository : IRepository<MDungeon>
    {

    }

    public class DungeonRepository : Repository<MDungeon>, IDungeonRepository
    {
        public DungeonRepository(AppDBContext context) : base(context)
        {
        }
    }

    public interface IDungeonRunRepository: IRepository<MDungeonRun>
    {
        Task<IEnumerable<MDungeonRun>> GetRunsByCharacterId(string characterId);
        Task<MDungeonRun?> GetbyIdWithDungeon(string runId);
    }

    public class DungeonRunRepository : Repository<MDungeonRun>, IDungeonRunRepository
    {
        public DungeonRunRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MDungeonRun?> GetbyIdWithDungeon(string runId)
        {
            return await _dbset.Include(dr => dr.Dungeon).FirstOrDefaultAsync(dr => dr.Id == runId);
        }

        public async Task<IEnumerable<MDungeonRun>> GetRunsByCharacterId(string characterId)
        {
            return await _dbset.Include(dr => dr.Dungeon).Where(dr => dr.CharacterId == characterId).ToListAsync();
        }
    }
}
