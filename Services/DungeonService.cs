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

    public interface IDungeonRunService
    {
        Task<ServiceResult<DungeonRunDTO>> StartDungeonRunAsync(string characterId, string dungeonId);
        Task<ServiceResult<DungeonRunDTO>> CompleteDungeonRunAsync(string runId, CompleteDungeonRunDTO completeDungeon);
        Task<ServiceResult<List<DungeonRunDTO>>> GetDungeonRunByCharacterAsync(string characterId);
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

    public class DungeonRunService : IDungeonRunService
    {

        private readonly IDungeonRepository _dungeonRepository;
        private readonly IDungeonRunRepository _dungeonRunRepository;
        private readonly ICharacterRepository _characterRepository;

        public DungeonRunService(IDungeonRepository dungeonRepository, IDungeonRunRepository dungeonRunRepository ,ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
            _dungeonRepository = dungeonRepository;
            _dungeonRunRepository = dungeonRunRepository;
        }

        public async Task<ServiceResult<DungeonRunDTO>> CompleteDungeonRunAsync(string runId, CompleteDungeonRunDTO completeDungeon)
        {
            var Rundb = await _dungeonRunRepository.GetbyIdWithDungeon(runId);

            if (Rundb == null)
            {
                return ServiceResult<DungeonRunDTO>.NotFound("No encontrado ningun intento con esa ID");
            }

            Rundb.IsSuccess = completeDungeon.IsSuccess;
            Rundb.CompletionTime = completeDungeon.CompletionTime;

            await _dungeonRunRepository.UpdateAsync(Rundb);

            var response = new DungeonRunDTO
            {
                id = runId,
                DungeonName = Rundb.Dungeon.Name,
                IsSuccess = Rundb.IsSuccess,
                CompletionTime = Rundb.CompletionTime,
                CreatedAt = Rundb.CreatedAt,
            };

            return ServiceResult<DungeonRunDTO>.Success(response);
        }

        public async Task<ServiceResult<List<DungeonRunDTO>>> GetDungeonRunByCharacterAsync(string characterId)
        {
            var characterDb = await _characterRepository.GetByIdAsync(characterId);
            if(characterDb == null)
            {
                return ServiceResult<List<DungeonRunDTO>>.NotFound("El personaje no fue encontrado");
            }

            var runs = await _dungeonRunRepository.GetRunsByCharacterId(characterId);

            var response = runs.Select(run => new DungeonRunDTO
            {
                id = run.Id,
                DungeonName = run.Dungeon.Name,
                IsSuccess = run.IsSuccess,
                CompletionTime = run.CompletionTime,
                CreatedAt = run.CreatedAt,
            }).ToList();

            return ServiceResult<List<DungeonRunDTO>>.Success(response);
        }

        public async Task<ServiceResult<DungeonRunDTO>> StartDungeonRunAsync(string characterId, string dungeonId)
        {
            var charDb = await _characterRepository.GetByIdAsync(characterId);
            if(charDb == null)
            {
                return ServiceResult<DungeonRunDTO>.NotFound("El personaje no fue encontrado");
            }

            var dungeonDb = await _dungeonRepository.GetByIdAsync(dungeonId);
            if(dungeonDb == null)
            {
                return ServiceResult<DungeonRunDTO>.NotFound("La mazmorra no fue encontrado");
            }


            var createRun = await _dungeonRunRepository.CreateAsync(new MDungeonRun
            {
                CompletionTime = 0,
                CharacterId = characterId,
                DungeonId = dungeonId,
            });

            var response = new DungeonRunDTO
            {
                id = createRun.Id,
                DungeonName = dungeonDb.Name,
                CompletionTime = createRun.CompletionTime,
                IsSuccess = createRun.IsSuccess,
                CreatedAt = createRun.CreatedAt,
            };

            return ServiceResult<DungeonRunDTO>.Success(response);
        }
    }
}
