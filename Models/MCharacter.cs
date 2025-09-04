using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DungeonCrawlerAPI.Models
{
    [Table("Character")]
    public class MCharacter:BaseEntity
    {
        [Required]
        [Column("Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column("Level")]
        public int Level { get; set; } = 1;

        [Column("Experience")]
        public int EXP { get; set; } = 1;

        [Column("Gold")]
        public int Gold { get; set; } = 0;

        [Column(TypeName = "json")]
        public string BaseStatsJson { get; set; }

        [NotMapped]
        public CharacterStats BaseStats
        {
            get => string.IsNullOrEmpty(BaseStatsJson)
                ? new CharacterStats()
                : JsonSerializer.Deserialize<CharacterStats>(BaseStatsJson);
            set => BaseStatsJson = JsonSerializer.Serialize(value);
        }

        // relaciones

        /// <summary>
        /// Usuario
        /// </summary>
        [Column("User")]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public MUser User { get; set; }

        /// <summary>
        /// Inventario
        /// <summary>
        [Column("Inventory")]
        public MInventory Inventory { get; set; }
        
        /// <summary>
        /// Dungeon Runs
        /// </summary>
        
        public ICollection<MDungeonRun> DungeonRuns { get; set; } = new List<MDungeonRun>();

        /// <summary>
        /// Items Solds
        /// </summary>
        
        public ICollection<ItemShop> ItemShop { get; set; } = new List<ItemShop>();

        /// <summary>
        /// Equipment Slots
        /// </summary>
        public ICollection<MEquipmentSlot> EquipmentSlots { get; set; }
    }

    public class CharacterStats
    {
        public int? Health { get; set; } = 100;
        public int? Mana { get; set; } = 50;
        public int? Strength { get; set; } = 10;
        public int? Dexterity { get; set; } = 10;
        public int? Intelligence { get; set; } = 10;
        public int? Vitality { get; set; } = 10;
        public int? Armor { get; set; } = 10;
        public int? MagicResist { get; set; } = 10;
        public int? CriticalChance { get; set; } = 10;
        public int? AttackSpeed { get; set; } = 10;
    }
}
