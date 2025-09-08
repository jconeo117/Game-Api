using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IDungeonService
    {
        Task<ServiceResult<DungeonDTO>> CreateDungeon(CreateDungeonDTO dungeonDTO);
        Task<ServiceResult<DungeonDTO>> UpdateDungeon(string dungeonId, UpdateDungeonDTO dungeonDTO);
        Task<ServiceResult<bool>> DeleteDungeon(string DungeonId);
    }

    public class DungeonService : IDungeonService
    {

        private readonly IDungeonRepository _dungeonRepository;

        public DungeonService(IDungeonRepository dungeonRepository)
        {
            _dungeonRepository = dungeonRepository;
        }
        public async Task<ServiceResult<DungeonDTO>> CreateDungeon(CreateDungeonDTO dungeonDTO)
        {
            var newDungeon = new MDungeon
            {
                Name = dungeonDTO.name,
                Description = dungeonDTO.description,
                Difficulty = dungeonDTO.difficulty,
            };

            var createdDungeon = await _dungeonRepository.CreateAsync(newDungeon);
            var response = new DungeonDTO
            {
                id = createdDungeon.Id,
                name = createdDungeon.Name,
                description = createdDungeon.Description,
                difficulty = createdDungeon.Difficulty
            };

            return ServiceResult<DungeonDTO>.Success(response);
        }

        public async Task<ServiceResult<bool>> DeleteDungeon(string DungeonId)
        {
            var DungeonToDelete = await _dungeonRepository.GetByIdAsync(DungeonId);
            if(DungeonToDelete == null)
            {
                return ServiceResult<bool>.NotFound("La dungeon a borrar no fue encontrada");
            }

            await _dungeonRepository.DeleteAsync(DungeonId);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<DungeonDTO>> UpdateDungeon(string dungeonId, UpdateDungeonDTO dungeonDTO)
        {
            var DungeonDB = await _dungeonRepository.GetByIdAsync(dungeonId);
            if (DungeonDB == null)
            {
                return ServiceResult<DungeonDTO>.NotFound("La dungeon no fue encontrada");
            }

            if (!String.IsNullOrEmpty(dungeonDTO.name))
            {
                DungeonDB.Name = dungeonDTO.name;
            }

            if (!String.IsNullOrEmpty(dungeonDTO.description))
            {
                DungeonDB.Description = dungeonDTO.description;
            }

            if (dungeonDTO.difficulty != null && DungeonDB.Difficulty != dungeonDTO.difficulty)
            {
                DungeonDB.Difficulty = (int)dungeonDTO.difficulty;
            }

            await _dungeonRepository.UpdateAsync(DungeonDB);

            var response = new DungeonDTO
            {
                id = dungeonId,
                name = DungeonDB.Name,
                description = DungeonDB.Description,
                difficulty = DungeonDB.Difficulty,
            };

            return ServiceResult<DungeonDTO>.Success(response);
        }
    }
}
