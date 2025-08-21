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
}
