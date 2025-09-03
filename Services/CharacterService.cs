using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;
using System;

namespace DungeonCrawlerAPI.Services
{
    public interface ICharacterService
    {
        Task<ServiceResult<CharacterDTO>> CreateCharacterAsync(string UserId, CreateCharacterDTO characterDTO);
        Task<ServiceResult<CharacterProfileDTO>> GetCharacterById(string CharacterId);
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
            var initialStats = new CharacterStats();
            var newChar = new MCharacter
            {
                Name = characterDTO.Name,
                UserId = UserId,
                Level = 1,
                CreatedBy = UserId,
                BaseStats = initialStats

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

        public async Task<ServiceResult<CharacterProfileDTO>> GetCharacterById(string CharacterId)
        {
            var CharDB = await _characterRepository.GetByIdAsync(CharacterId);

            if(CharDB == null)
            {
                return ServiceResult<CharacterProfileDTO>.NotFound("El personahe no fue encontrado");
            }

            var response = new CharacterProfileDTO
            {
                Id = CharDB.Id,
                Name = CharDB.Name,
                Level = CharDB.Level,
                Experience = CharDB.EXP,
                Gold = CharDB.Gold,
                CharacterStats = new CharacterStatsDTO
                {
                    Health = CharDB.BaseStats.Health ?? 0,
                    Mana = CharDB.BaseStats.Mana ?? 0,
                    Strength = CharDB.BaseStats.Strength ?? 0,
                    Dexterity = CharDB.BaseStats.Dexterity ?? 0,
                    Intelligence = CharDB.BaseStats.Intelligence ?? 0,
                    Vitality = CharDB.BaseStats.Vitality ?? 0,
                    Armor = CharDB.BaseStats.Armor ?? 0,
                    MagicResist = CharDB.BaseStats.MagicResist ?? 0,
                    CriticalChance = CharDB.BaseStats.CriticalChance ?? 0,
                    AttackSpeed = CharDB.BaseStats.AttackSpeed ?? 0
                }
            };

            return ServiceResult<CharacterProfileDTO>.Success(response);
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
