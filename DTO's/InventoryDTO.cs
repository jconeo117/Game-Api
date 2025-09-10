
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.DTO_s
{
    public class InventoryDTO
    {
        public string Id { get; set; }
        public int NumSlots { get; set; }
        public List<ItemDTO> Items { get; set; } = new();
    }

    public class EquipItemRequestDTO
    {
        public string ItemId { get; set; }
        public EquipmentSlotType SlotType { get; set; }
    }

    public class UnequipItemRequestDTO
    {
        public EquipmentSlotType SlotType { get; set; }
    }
}
