using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
using DungeonCrawlerAPI.Models;
using System;

namespace DungeonCrawlerAPI.Services
{
    public interface ICharacterService
    {
        Task<ServiceResult<CharacterDTO>> CreateCharacterAsync(string UserId, CreateCharacterDTO characterDTO);
        Task<ServiceResult<CharacterDTO>> GetCharacterById(string CharacterId);
        Task<ServiceResult<CharacterProfileDTO>> GetCharacterProfileById(string CharacterId);
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

        public async Task<ServiceResult<CharacterDTO>> GetCharacterById(string CharacterId)
        {
            var CharDB = await _characterRepository.GetByIdAsync(CharacterId);

            if(CharDB == null)
            {
                return ServiceResult<CharacterDTO>.NotFound("El personahe no fue encontrado");
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

        public async Task<ServiceResult<CharacterProfileDTO>> GetCharacterProfileById(string CharacterId)
        {
            var characterProf = await _characterRepository.GetCharacterProfileByIdAsync(CharacterId);

            if(characterProf == null)
            {
                return ServiceResult<CharacterProfileDTO>.NotFound("El personaje no fue encontrado");
            }
            var response = new CharacterProfileDTO
            {
                Id = characterProf.Id,
                Name = characterProf.Name,
                Level = characterProf.Level,
                Experience = characterProf.EXP,
                Gold = characterProf.Gold,
                CharacterStats = CalculateStats(characterProf),
                EquipmentSlots = EquipmentSlotMap(characterProf)
            };

            return ServiceResult<CharacterProfileDTO>.Success(response);
        }

        private CharacterStatsDTO CalculateStats(MCharacter character)
        {
            var totalstats = new CharacterStats
            {
                Health = character.BaseStats.Health,
                Mana = character.BaseStats.Mana,
                Strength = character.BaseStats.Strength,
                Dexterity = character.BaseStats.Dexterity,
                Intelligence = character.BaseStats.Intelligence,
                Vitality = character.BaseStats.Vitality,
                Armor = character.BaseStats.Armor,
                MagicResist = character.BaseStats.MagicResist,
                CriticalChance = character.BaseStats.CriticalChance,
                AttackSpeed = character.BaseStats.AttackSpeed
            };

            foreach (var slot in character.EquipmentSlots)
            {
                if(slot.Item != null)
                {
                    totalstats.Health += slot.Item.Stats.Health ?? 0;
                    totalstats.Mana += slot.Item.Stats.Mana ?? 0;
                    totalstats.Strength += slot.Item.Stats.Strength ?? 0;
                    totalstats.Dexterity += slot.Item.Stats.Dexterity ?? 0;
                    totalstats.Intelligence += slot.Item.Stats.Intelligence ?? 0;
                    totalstats.Vitality += slot.Item.Stats.Vitality ?? 0;
                    totalstats.Armor += slot.Item.Stats.Armor ?? 0;
                    totalstats.MagicResist += slot.Item.Stats.MagicResist ?? 0;
                    totalstats.CriticalChance += slot.Item.Stats.CriticalChance ?? 0;
                    totalstats.AttackSpeed += slot.Item.Stats.AttackSpeed ?? 0;
                }
            }

            var response = new CharacterStatsDTO
            {
                Health = (int)totalstats.Health,
                Mana = (int)totalstats.Mana,
                Strength = (int)totalstats.Strength,
                Dexterity = (int)totalstats.Dexterity,
                Intelligence = (int)totalstats.Intelligence,
                Vitality = (int)totalstats.Vitality,
                Armor = (int)totalstats.Armor,
                MagicResist = (int)totalstats.MagicResist,
                CriticalChance = (int)totalstats.CriticalChance,
                AttackSpeed = (int)totalstats?.AttackSpeed,
            };

            return response;
        }

        private List<EquipmentSlotDTO> EquipmentSlotMap(MCharacter character)
        {
            var EquipmentSlotsList = new List<EquipmentSlotDTO>();

            foreach (var equipmentSlot in character.EquipmentSlots)
            {
                var slotDTO = new EquipmentSlotDTO
                {
                    SlotType = equipmentSlot.SlotType.ToString(),
                    Item = null
                };
                
                if(equipmentSlot.Item != null)
                {
                    slotDTO.Item = new ItemSlotDTO
                    {
                        name = equipmentSlot.Item.Name,
                        ItemType = equipmentSlot.Item.ItemType.ToString(),
                        stats = equipmentSlot.Item.Stats.GetActiveStats(),
                    };
                }
                EquipmentSlotsList.Add(slotDTO);
            }

            return EquipmentSlotsList;
        }
    }
}
