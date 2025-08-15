using DungeonCrawlerAPI.DTO_s;
using DungeonCrawlerAPI.Interfaces;
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

        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public AuthService (IAuthRepository authRepository, ITokenService tokenService, IPasswordService passwordService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

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

        public async Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO user)
        {
            var existUser = await _authRepository.GetByEmailAsync(user.Email);
            if(existUser == null)
            {
                return ServiceResult<AuthResponseDTO>.Error("No hay usuario registrado con este email.");
            }

            if(!_passwordService.ValidatePassword(user.Password, existUser.Password))
            {
                return ServiceResult<AuthResponseDTO>.Error("La contraseña ingresada es incorrecta.");
            }

            var userProfile = new UserDTO
            {
                Id = existUser.Id,
                Username = existUser.Username,
                Email = existUser.Email,
                CreatedAt = existUser.CreatedAt
            };

            var accessToken = _tokenService.GenerateToken(existUser);
            var refreshToken = _tokenService.RefreshToken();

            
            existUser.RefreshToken = refreshToken;
            existUser.TokenExpiresTimen = DateTime.UtcNow.AddDays(7);
            existUser.LastLogin = DateTime.UtcNow;

            await _authRepository.UpdateAsync(existUser);

            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 60 * 60,
                UserProfile = userProfile
            };

            return ServiceResult<AuthResponseDTO>.Success(response);
        }

        public async Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                return ServiceResult<AuthResponseDTO>.Error("Las contraseñas no coinciden.");
            }

            var existingUserByEmail = await _authRepository.GetByEmailAsync(user.Email);
            if (existingUserByEmail != null)
            {
                return ServiceResult<AuthResponseDTO>.Error("El email ya está en uso.");
            }

            var existingUserByUsername = await _authRepository.GetByUsernameAsync(user.UserName);
            if (existingUserByUsername != null)
            {
                return ServiceResult<AuthResponseDTO>.Error("El nombre de usuario ya está en uso.");
            }


            var newUser = new MUser
            {
                Email = user.Email,
                Username = user.UserName,
                Password = _passwordService.HashPassword(user.Password),
                CreatedBy = user.Email
            };

            var createdUser = await _authRepository.CreateAsync(newUser);
            if(createdUser == null)
            {
                return ServiceResult<AuthResponseDTO>.Error("La cuenta no pudo crearse");
            }

            var accessToken = _tokenService.GenerateToken(newUser);
            var refreshToken = _tokenService.RefreshToken();
            
            createdUser.RefreshToken = refreshToken;
            createdUser.TokenExpiresTimen = DateTime.UtcNow.AddDays(7);

            await _authRepository.UpdateAsync(createdUser);

            var userProfile = new UserDTO
            {
                Id = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt,
            };

            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 60 * 60,
                UserProfile = userProfile
            };

            return ServiceResult<AuthResponseDTO>.Success(response);
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
