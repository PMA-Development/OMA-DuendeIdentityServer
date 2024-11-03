using Duende.IdentityServer.Models;
using System.Security.Cryptography;

namespace OMA_DuendeIdentityServer.Pages.ClientManagement
{
    public class SecretGenerator
    {
        public static string GenerateRandomSecret()
        {
            // Generates a random 32-byte secret
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public static string HashSecret(string secret)
        {
            // Hash the secret using Sha256
            return secret.Sha256();
        }
    }
}
