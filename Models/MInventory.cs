using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonCrawlerAPI.Models
{
    [Table("Inventory")]
    public class MInventory:BaseEntity
    {
        [ForeignKey(nameof(Player))]
        public string UserId { get; set; }
        public int NumSlots { get; set; } = 16;

        public ICollection<MItems> Items { get; set; } = new List<MItems>();

        public MCharacter Player { get; set; }

    }
}
