using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonCrawlerAPI.Models
{
    public enum EquipmentSlotType
    {
        Weapon,
        Shield,
        Helmet,
        Chest,
        Legs,
        Boots,
        Ring1,
        Ring2
    }

    [Table("EquipmentSlots")]
    public class MEquipmentSlot : BaseEntity
    {
        [Required]
        public string CharacterId { get; set; }

        [ForeignKey(nameof(CharacterId))]
        public MCharacter Character { get; set; }

        [Required]
        public EquipmentSlotType SlotType { get; set; }

        // El ItemId puede ser nulo si el slot está vacío
        public string? ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public MItems Item { get; set; }
    }
}
