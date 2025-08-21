
namespace DungeonCrawlerAPI.DTO_s
{
    public class InventoryDTO
    {
        public string Id { get; set; }
        public int NumSlots { get; set; }
        public List<ItemDTO> Items { get; set; } = new();
    }
}
