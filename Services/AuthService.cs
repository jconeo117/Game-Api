using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IAuthService
    {
        Task<ServiceResult<UserDTO>> RegisterAsync(RegisterDTO user);
        Task<ServiceResult<UserDTO>> LoginAsync(LoginDTO user);
        Task<ServiceResult<UserDTO>> UpdateProfileAsync(string id, UserDTO user);
        Task<ServiceResult<bool>> DeleteProfileAsync(string id);

        Task<ServiceResult<bool>> ChangePasswordAsync(string id, ChangePasswordDTO changePasswordDTO);
        Task<ServiceResult<UserDTO>> GetProfileAsync(string id);

        Task<ServiceResult<bool>> ValidateUsernameExistsAsync(string username);
        Task<ServiceResult<bool>> ValidateEmailExistsAsync(string Email);
    }


    public class AuthService
    {
    }
}
