using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FilmApp.API.Helpers
{
    public static class PasswordHasher
    {
        /// <summary>Şifreyi hash'ler ve salt ile birlikte string olarak döner.</summary>
        public static string Hash(string password)
        {
            // Rastgele salt üret
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Şifreyi hashle
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Salt ve hash'i birleştirerek döndür
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        /// <summary>Gelen şifreyi, saklanan hash+salt ile karşılaştırır.</summary>
        public static bool Verify(string password, string hashedWithSalt)
        {
            var parts = hashedWithSalt.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            // Aynı salt ile yeniden hashle
            string hashToCompare = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashToCompare == storedHash;
        }
    }
}