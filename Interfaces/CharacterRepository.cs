using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface ICharacterRepository : IRepository<MCharacter>
    {
        Task<MCharacter?> GetCharByUserIdAsync(string UserId);
        Task<MCharacter?> GetByNameAsync(string Name, string UserId);
    }

    public class CharacterRepository : Repository<MCharacter>, ICharacterRepository
    {
        public CharacterRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MCharacter?> GetByNameAsync(string Name, string UserId)
        {
            return await _dbset.FirstOrDefaultAsync(c => c.Name == Name && c.UserId == UserId);
        }

        public async Task<MCharacter?> GetCharByUserIdAsync(string UserId)
        {
            return await _dbset.FirstOrDefaultAsync(c => c.UserId == UserId);
        }
    }
}
