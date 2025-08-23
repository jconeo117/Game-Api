using System.ComponentModel.DataAnnotations;

namespace DungeonCrawlerAPI.DTO_s
{
    public class ItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public int value { get; set; }
        public Dictionary<string, int> stats { get; set; }
    }

    public class ItemStatsDTO
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
    }

    /// <summary>
    /// DTO para crear una nueva plantilla de ítem. Todos los campos son requeridos.
    /// </summary>
    public class CreateItemDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El tipo de ítem es obligatorio.")]
        // Se valida en el servicio que el string corresponda a un valor del enum ItemType
        public string ItemType { get; set; }

        [Required(ErrorMessage = "El valor es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor no puede ser negativo.")]
        public int Value { get; set; }

        [Required]
        public ItemStatsDTO Stats { get; set; }
    }

    public class UpdateItemDTO
    {
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string ItemType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El valor no puede ser negativo.")]
        public int? Value { get; set; } // Nullable para que no sea obligatorio

        public ItemStatsDTO Stats { get; set; }
    }
}
