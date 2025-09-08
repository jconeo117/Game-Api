namespace DungeonCrawlerAPI.DTO_s
{
    public class DungeonDTO
    {
        public string id {  get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }
    }

    public class CreateDungeonDTO
    {
        public string name { get; set;}
        public string description { get; set;}
        public int difficulty { get; set; }
    }

    public class UpdateDungeonDTO
    {
        public string? name { get; set;}
        public string? description { get; set;}
        public int? difficulty { get; set; }
    }

    public class DungeonRunDTO
    {
        public string id { get; set;}
        public string DungeonName { get; set;}
        public bool IsSuccess { get; set;}
        public int CompletionTime { get; set;}
        public DateTime CreatedAt { get; set;}
    }

    public class CompleteDungeonRunDTO
    {
        public bool IsSuccess { get; set;}
        public int CompletionTime { get; set;}
    }
}
