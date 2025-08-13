namespace DungeonCrawlerAPI.Services
{

    public interface IPasswordService
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string hash);

    }


    public class PasswordService : IPasswordService
    {

        private const int DefaultConstFactor = 12;

        public string HashPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacia");
            }

            return BCrypt.Net.BCrypt.HashPassword(password, DefaultConstFactor);
        }

        public bool ValidatePassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception)
            {
                // Log el error si tienes logging
                return false;
            }
        }
    }
}
