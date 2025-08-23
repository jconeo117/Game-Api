using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IItemService
    {
        Task<ServiceResult<List<ItemDTO>>> GetAllItemsAsync();
        Task<ServiceResult<ItemDTO>> GetItemByIdAsync(string itemId);
        Task<ServiceResult<ItemDTO>> CreateItemAsync(CreateItemDTO createDto);
        Task<ServiceResult<ItemDTO>> UpdateItemAsync(string itemId, UpdateItemDTO updateDto);
        Task<ServiceResult<bool>> DeleteItemAsync(string itemId);
    }
    public class ItemService : IItemService
    {
        public Task<ServiceResult<ItemDTO>> CreateItemAsync(CreateItemDTO createDto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> DeleteItemAsync(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<List<ItemDTO>>> GetAllItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ItemDTO>> GetItemByIdAsync(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ItemDTO>> UpdateItemAsync(string itemId, UpdateItemDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
