using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Interfaces
{

    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }



    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        protected readonly AppDBContext _context;
        protected readonly DbSet<T> _dbset;

        public Repository(AppDBContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _dbset.FindAsync(id);

            if(entity == null)
            {
                return false;
            }

            _dbset.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _dbset.AnyAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
