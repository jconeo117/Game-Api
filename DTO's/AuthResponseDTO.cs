namespace DungeonCrawlerAPI.DTO_s
{
    // TokenDto.cs
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }

        public UserDTO UserProfile { get; set; }
    }
}
