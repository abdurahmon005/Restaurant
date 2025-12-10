using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Helpers.PasswordHash
{
    public interface IPasswordHash
    {
        string GenerateSalt();
        string Encrypt(string password, string salt);
        bool Verify(string hash, string password, string salt);
    }
}
