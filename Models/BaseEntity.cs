using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonCrawlerAPI.Models
{
    public  abstract class BaseEntity
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("Created_At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("Updated_At")]
        
        public DateTime? UpdatedAt { get; set; }
        
        [Column("Created_By")]
        public string CreatedBy { get; set; }
        
        [Column("Updated_By")]
        public string? UpdatedBy { get; set; }

        [Column("Is_Deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
