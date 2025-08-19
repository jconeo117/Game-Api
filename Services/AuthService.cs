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

        Task<ServiceResult<bool>> ValidateUsernameExistsAsync(string username, string excludeUserId);
        Task<ServiceResult<bool>> ValidateEmailExistsAsync(string Email, string excludeUserId);
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

        public async Task<ServiceResult<bool>> ChangePasswordAsync(string id, ChangePasswordDTO changePasswordDTO)
        {

            var user = await _authRepository.GetByIdAsync(id);

            if(user == null)
            {
                return ServiceResult<bool>.NotFound("Usuario no encontrado");
            }

            if(!_passwordService.ValidatePassword(changePasswordDTO.CurrentPassword, user.Password))
            {
                return ServiceResult<bool>.Error("La contraseña actual no es correcta");
            }

            if(changePasswordDTO.NewPassword != changePasswordDTO.ConfirmNewPassword)
            {
                return ServiceResult<bool>.Error("Las contraseñas no coinciden");
            }

            var newPassword = _passwordService.HashPassword(changePasswordDTO.ConfirmNewPassword);

            user.Password = newPassword;
            await _authRepository.UpdateAsync(user);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteProfileAsync(string id)
        {
            var user = await _authRepository.GetByIdAsync(id);
            if(user == null)
            {
                return ServiceResult<bool>.Error("El usuario no fue encontrado");
            }

            await _authRepository.DeleteAsync(id);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<UserDTO>> GetProfileAsync(string id)
        {
            var profile = await _authRepository.GetByIdAsync(id);

            if(profile == null)
            {
                return ServiceResult<UserDTO>.NotFound("El usuario no fue encontrado");
            }

            var Response = new UserDTO
            {
                Email = profile.Email,
                Username = profile.Username,
                Id = profile.Id,
                CreatedAt = profile.CreatedAt
            };

            return ServiceResult<UserDTO>.Success(Response);
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

        public async Task<ServiceResult<UserDTO>> UpdateProfileAsync(string id, UserDTO user)
        {
            var userDB = await _authRepository.GetByIdAsync(id);

            if(userDB == null)
            {
                return ServiceResult<UserDTO>.NotFound("El usuario no fue encontrado");
            }

            bool emailChanged = userDB.Email != user.Email;
            bool usernameChanged = userDB.Username != user.Username;

            // 3. Si no hay cambios, retornar el usuario actual
            if (!emailChanged && !usernameChanged)
            {
                return ServiceResult<UserDTO>.Success(user);
            }

            var validationTasks = new List<Task<ServiceResult<bool>>>();

            if (emailChanged)
                validationTasks.Add(ValidateEmailExistsAsync(user.Email, id));

            if (usernameChanged)
                validationTasks.Add(ValidateUsernameExistsAsync(user.Username, id));

            var validationResults = await Task.WhenAll(validationTasks);

            // 5. Verificar si alguna validación falló
            var failedValidation = validationResults.FirstOrDefault(r => !r.IsSuccess);
            if (failedValidation != null)
            {
                return ServiceResult<UserDTO>.Error(failedValidation.ErrorMessage);
            }

            if (emailChanged)
                userDB.Email = user.Email;

            if (usernameChanged)
                userDB.Username = user.Username;

            userDB.UpdatedAt = DateTime.UtcNow;

            await _authRepository.UpdateAsync(userDB);

            var response = new UserDTO
            {
                Id = userDB.Id,
                Email = userDB.Email,
                Username = userDB.Username,
                CreatedAt = userDB.CreatedAt,
            };

            return ServiceResult<UserDTO>.Success(response);
        }

        public async Task<ServiceResult<bool>> ValidateEmailExistsAsync(string Email, string excludeUserId)
        {
            var existingUser = await _authRepository.GetByEmailAsync(Email);

            // Si existe un usuario con ese email Y no es el usuario actual, entonces está en uso
            if (existingUser != null && existingUser.Id != excludeUserId)
            {
                return ServiceResult<bool>.Error("Este email ya está en uso");
            }

            return ServiceResult<bool>.Success(true); // Email disponible
        }

        public async Task<ServiceResult<bool>> ValidateUsernameExistsAsync(string username, string excludeUserId)
        {
            var existingUser = await _authRepository.GetByUsernameAsync(username);

            // Si existe un usuario con ese email Y no es el usuario actual, entonces está en uso
            if (existingUser != null && existingUser.Id != excludeUserId)
            {
                return ServiceResult<bool>.Error("Este username ya está en uso");
            }

            return ServiceResult<bool>.Success(true); // Email disponible
        }
    }
}
