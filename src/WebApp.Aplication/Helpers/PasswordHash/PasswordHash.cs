using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Helpers.PasswordHash
{
    public class PasswordHash : IPasswordHash
    {
        public string Encrypt(string password, string salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(
            password: password,
            salt: Encoding.UTF8.GetBytes(salt),
            iterations: 3,
            hashAlgorithm: HashAlgorithmName.SHA256);

            return Convert.ToBase64String(algorithm.GetBytes(32));
        }

        public bool Verify(string password, string salt, string hash)
        {
            var newHash = Encrypt(password, salt);
            return newHash == hash;
        }
        

        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }

        
    }
}
