
using DungeonCrawlerAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DungeonCrawlerAPI.Services
{

    public interface ITokenService
    {
        string GenerateToken(MUser User);
        string RefreshToken();
    }
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(MUser User)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.Id),
                new Claim(ClaimTypes.Name, User.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, User.Role.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:AccessTokenExpirationMinutes"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);

        }

        public string RefreshToken()
        {
            var randomNumber = new byte[32]; // 32 bytes = 256 bits

            // Usamos el generador de números aleatorios criptográficamente seguro.
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                // Convertimos el array de bytes a un string en formato Base64 para que sea fácil de guardar y enviar.
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
