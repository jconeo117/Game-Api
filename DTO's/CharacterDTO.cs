using System.ComponentModel.DataAnnotations;

namespace DungeonCrawlerAPI.DTO_s
{

    public class CreateCharacterDTO
    {
        [Required(ErrorMessage = "El nombre del personaje es obligatorio.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Name { get; set; }
    }

    public class CharacterDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public string Owner { get; set; }
    }
}
