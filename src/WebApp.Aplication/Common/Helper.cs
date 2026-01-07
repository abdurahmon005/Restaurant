using System.Security.Cryptography;
using System.Text;

namespace WebApp.Aplication.Common
{
    public class Helper
    {
        private const int Iterations = 100000;
        private const int KeyLength = 32;

        public string Encript(string password, string salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256);

            return Convert.ToBase64String(algorithm.GetBytes(KeyLength));
        }

        public bool Verify(string hash, string password, string salt)
        {
            var newHash = Encript(password, salt);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(newHash),
                Encoding.UTF8.GetBytes(hash));
        }
    }
}

