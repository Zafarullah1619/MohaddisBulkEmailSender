using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Org.Business.Objects
{
    public class PasswordHelper
    {
        public static string HashedPassword { get; set; }
        public static string Salt { get; set; }
        public static string UserPassword { get; set; }
        
        private static void SetPassword(ref string password)
        {
            if ((password == null || password.Length <= 0) && (UserPassword == null || UserPassword.Length <= 0))
            {
                throw new ArgumentNullException("UserPassword");
            }

            if (password == null || password.Length <= 0)
            {
                password = UserPassword;
            }
            else
            {
                UserPassword = password;
            }
        }

        private static void SetHashedPassword(ref string hashedPassword)
        {
            if ((hashedPassword == null || hashedPassword.Length <= 0) && (HashedPassword == null || HashedPassword.Length <= 0))
            {
                throw new ArgumentNullException("HashedPassword");
            }

            if (hashedPassword == null || hashedPassword.Length <= 0)
            {
                hashedPassword = HashedPassword;
            }
            else
            {
                HashedPassword = hashedPassword;
            }
        }

        private static void SetSalt(ref string salt)
        {
            if ((salt == null || salt.Length <= 0) && (Salt == null || Salt.Length <= 0))
            {
                throw new ArgumentNullException("HashedPassword");
            }

            if (salt == null || salt.Length <= 0)
            {
                salt = Salt;
            }
            else
            {
                Salt = salt;
            }
        }

        public static void HashPassword(string password = null)
        {
            SetPassword(ref password);

            byte[] saltBytes = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            Salt = Convert.ToBase64String(saltBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword = null, string password = null, string salt = null)
        {
            string NewHashedPassword;

            try
            {
                SetPassword(ref password);
                SetHashedPassword(ref hashedPassword);
                SetSalt(ref salt);

                byte[] saltBytes = Convert.FromBase64String(Salt);

                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                NewHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: saltBytes,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: (256 / 8)));

                if (NewHashedPassword == HashedPassword)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }

            return false;
        }
    }
}
