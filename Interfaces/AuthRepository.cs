using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IAuthRepository : IRepository<MUser>
    {
        Task<MUser?> GetByEmailAsync(string email);
        Task<MUser?> GetByUsernameAsync(string username);

    }


    public class AuthRepository : Repository<MUser>, IAuthRepository
    {
        public AuthRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<MUser?> GetByEmailAsync(string email)
        {
            return await _dbset.FirstOrDefaultAsync(us =>  us.Email == email);
        }

        public async Task<MUser?> GetByUsernameAsync(string username)
        {
            return await _dbset.FirstOrDefaultAsync(us =>  us.Username == username);
        }
    }
}
