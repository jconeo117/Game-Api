using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Models;

namespace DungeonCrawlerAPI.Services
{

    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO user);
        Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO user);
        Task<ServiceResult<UserDTO>> UpdateProfileAsync(string id, UserDTO user);
        Task<ServiceResult<bool>> DeleteProfileAsync(string id);

        Task<ServiceResult<bool>> ChangePasswordAsync(string id, ChangePasswordDTO changePasswordDTO);
        Task<ServiceResult<UserDTO>> GetProfileAsync(string id);

        Task<ServiceResult<bool>> ValidateUsernameExistsAsync(string username);
        Task<ServiceResult<bool>> ValidateEmailExistsAsync(string Email);
    }


    public class AuthService : IAuthService
    {
        public Task<ServiceResult<bool>> ChangePasswordAsync(string id, ChangePasswordDTO changePasswordDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> DeleteProfileAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<UserDTO>> GetProfileAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<UserDTO>> UpdateProfileAsync(string id, UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> ValidateEmailExistsAsync(string Email)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> ValidateUsernameExistsAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
