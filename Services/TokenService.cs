
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface ITokenService
    {
        string GenerateToken(MUser User);
        string RefreshToken();
    }
    public class TokenService
    {
    }
}
