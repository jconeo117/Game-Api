using System.ComponentModel.DataAnnotations;

namespace DungeonCrawlerAPI.DTO_s
{

    public class CreateCharacterDTO
    {
        [Required(ErrorMessage = "El nombre del personaje es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Name { get; set; }
    }

    public class CharacterDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }

    }
    public class CharacterProfileDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public CharacterStatsDTO CharacterStats { get; set; }
        public List<EquipmentSlotDTO> EquipmentSlots { get; set; } = new();

    }

    public class EquipmentSlotDTO
    {
        public string SlotType { get; set; }
        public ItemSlotDTO Item { get; set; }
    }

    public class CharacterStatsDTO
    {
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Vitality { get; set; }
        public int Armor { get; set; }
        public int MagicResist { get; set; }
        public int CriticalChance { get; set; }
        public int AttackSpeed { get; set; }
    }
}
