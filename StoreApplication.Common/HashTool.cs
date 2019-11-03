using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace StoreApplication.Common
{
    // simple hashing class, used to hash passwords and IDs stored in cookies
    public class HashTool
    {
        public static string HashString(string input)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: Encoding.UTF8.GetBytes(AppConfig.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
