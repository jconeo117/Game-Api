using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{
    public interface ICharacterService
    {
        Task<ServiceResult<CharacterDTO>> CreateCharacterAsync(string UserId, CreateCharacterDTO characterDTO);
        Task<ServiceResult<CharacterDTO>> GetCharacterById(string CharacterId);
        Task<ServiceResult<CharacterDTO>> GetCharacterByUserId(string UserId);

    }

    public class CharacterService : ICharacterService
    {

        private readonly ICharacterRepository _characterRepository;   
        private readonly IInventoryService _inventoryService;

        public CharacterService(ICharacterRepository characterRepository, IInventoryService inventoryService)
        {
            _characterRepository = characterRepository;
            _inventoryService = inventoryService;
        }

        public async Task<ServiceResult<CharacterDTO>> CreateCharacterAsync(string UserId, CreateCharacterDTO characterDTO)
        {
            var newChar = new MCharacter
            {
                Name = characterDTO.Name,
                UserId = UserId,
                Level = 1,
                CreatedBy = UserId

            };
            var createdChar = await _characterRepository.CreateAsync(newChar);
            await _inventoryService.CreateNewInventoryAsync(createdChar.Id);

            var response = new CharacterDTO
            {
                Id = createdChar.Id,
                Name = createdChar.Name,
                Level = createdChar.Level,
                Experience = createdChar.EXP,
                Gold = createdChar.Gold,
            };
            return ServiceResult<CharacterDTO>.Success(response);
        }

        public Task<ServiceResult<CharacterDTO>> GetCharacterById(string CharacterId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<CharacterDTO>> GetCharacterByUserId(string UserId)
        {
            var CharDB = await _characterRepository.GetCharByUserIdAsync(UserId);
            
            if(CharDB == null)
            {
                return ServiceResult<CharacterDTO>.Error("El usuario no tiene ningun personaje");
            }

            var response = new CharacterDTO
            {
                Id = CharDB.Id,
                Name = CharDB.Name,
                Level = CharDB.Level,
                Experience = CharDB.EXP,
                Gold = CharDB.Gold,
            };

            return ServiceResult<CharacterDTO>.Success(response);

        }
    }
}
