using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


    }
}
