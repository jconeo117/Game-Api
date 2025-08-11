using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonCrawlerAPI.Models
{
    [Table("Dungeons")]
    public class MDungeon : BaseEntity
    {
        [Required]
        [Column("Dungeon_Name")]
        public string Name { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }

        [Column("Difficulty")]
        public int Difficulty { get; set; } = 1;

        public ICollection<MDungeonRun> Runs { get; set; } = new List<MDungeonRun>();
    }

    public class MDungeonRun : BaseEntity
    {

        [Column("Success")]
        public bool IsSuccess { get; set; } = false;

        [Column("Completion_Time")]
        public int CompletionTime { get; set; }

        [Column("Dungeon_Id")]
        public string DungeonId { get; set; }

        [ForeignKey(nameof(DungeonId))]
        public MDungeon Dungeon { get; set; }

        [Column("Character_Id")]
        public string CharacterId { get; set; }

        [ForeignKey(nameof(CharacterId))]
        public MCharacter Character { get; set; }
    }
}
