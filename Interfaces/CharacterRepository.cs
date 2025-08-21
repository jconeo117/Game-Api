using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface ICharacterRepository : IRepository<MCharacter>
    {
        Task<MCharacter?> GetCharByIdAsync(string UserId);
        Task<MCharacter?> GetByNameAsync(string Name);
    }

    public class CharacterRepository : Repository<MCharacter>, ICharacterRepository
    {
        public CharacterRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MCharacter?> GetByNameAsync(string Name)
        {
            return await _dbset.FirstOrDefaultAsync(c => c.Name == Name);
        }

        public async Task<MCharacter?> GetCharByIdAsync(string CharId)
        {
            return await _dbset.FirstOrDefaultAsync(c => c.Id == CharId);
        }
    }
}
