using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonCrawlerAPI.Models
{

    [Table("Players")]
    public class MUser : BaseEntity
    {
        [Required]
        [Column("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("Username")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Email_Verified")]
        public bool EmailVerify { get; set; } = false;

        [Column("Last_Login")]
        public DateTime? LastLogin { get; set; }

        [Column("Is_Active")]
        public bool IsActive { get; set; } = false;

        [Column("Character")]
        public MCharacter Character { get; set; }
    }
}
