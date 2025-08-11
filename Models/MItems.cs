using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DungeonCrawlerAPI.Models
{
    [Table("Items")]
    public class MItems : BaseEntity
    {
        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }

        [Required]
        [Column("Item_Type")]
        public ItemType ItemType { get; set; }

        [Column("Value")]
        public int Value { get; set; }

        [Column(TypeName = "json")]
        public string StatsJson { get; set; }

        // Propiedad no mapeada para trabajar con las stats
        [NotMapped]
        public ItemStats Stats
        {
            get => string.IsNullOrEmpty(StatsJson)
                ? new ItemStats()
                : JsonSerializer.Deserialize<ItemStats>(StatsJson);
            set => StatsJson = JsonSerializer.Serialize(value);
        }

        //Relaciones
        [Column("Inventario")]
        public string InventaryId { get; set; }

        [ForeignKey(nameof(InventaryId))]
        public MInventory Inventory { get; set; }
    }

    [Table("Items_Shop")]
    public class ItemShop : BaseEntity
    {
        [Column("Stock")]
        public int Stock { get; set; } = 1;

        [Column("Value")]
        public int Value { get; set; }

        ///
        ///Relaciones
        ///

        [Column("Owner_Id")]
        public string CharacterId { get; set; }
        
        [ForeignKey(nameof(CharacterId))]
        public MCharacter Character { get; set; }

        [Column("Item_Id")]
        public string ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public MItems Item { get; set; }
    }
}

public class ItemStats
{
    public int? Health { get; set; }
    public int? Mana { get; set; }
    public int? Strength { get; set; }
    public int? Dexterity { get; set; }
    public int? Intelligence { get; set; }
    public int? Vitality { get; set; }
    public int? Armor { get; set; }
    public int? MagicResist { get; set; }
    public int? CriticalChance { get; set; }
    public int? AttackSpeed { get; set; }

    // Método helper para obtener stats no nulas
    public Dictionary<string, int> GetActiveStats()
    {
        var stats = new Dictionary<string, int>();

        if (Health.HasValue) stats["Health"] = Health.Value;
        if (Mana.HasValue) stats["Mana"] = Mana.Value;
        if (Strength.HasValue) stats["Strength"] = Strength.Value;
        if (Dexterity.HasValue) stats["Dexterity"] = Dexterity.Value;
        if (Intelligence.HasValue) stats["Intelligence"] = Intelligence.Value;
        if (Vitality.HasValue) stats["Vitality"] = Vitality.Value;
        if (Armor.HasValue) stats["Armor"] = Armor.Value;
        if (MagicResist.HasValue) stats["MagicResist"] = MagicResist.Value;
        if (CriticalChance.HasValue) stats["CriticalChance"] = CriticalChance.Value;
        if (AttackSpeed.HasValue) stats["AttackSpeed"] = AttackSpeed.Value;

        return stats;
    }
}

public enum ItemType
{
    Weapon,
    Armor,
    Consumible
}