using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface ICharacterRepository : IRepository<MCharacter>
    {
        Task<MCharacter?> GetCharByUserIdAsync(string UserId);
        Task<MCharacter?> GetByNameAsync(string Name, string UserId);
        Task<MCharacter?> GetCharacterProfileByIdAsync(string CharacterId);
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

        public async Task<MCharacter?> GetCharacterProfileByIdAsync(string CharacterId)
        {
            return await _dbset.Include(c => c.EquipmentSlots).ThenInclude(es => es.Item).FirstOrDefaultAsync(c => c.Id == CharacterId);
        }

        public async Task<MCharacter?> GetCharByUserIdAsync(string UserId)
        {
            return await _dbset.FirstOrDefaultAsync(c => c.UserId == UserId);
        }
    }
}
