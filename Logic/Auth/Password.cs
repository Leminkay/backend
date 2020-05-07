using System;
using System.Security.Cryptography;
using System.Text;

namespace Auth
{
    public class Password
    {
        public Password(string password)
        {
            byte[] saltBytes = new byte[32];
            using (var rand = RandomNumberGenerator.Create())
            {
                rand.GetBytes(saltBytes);
            }
            Salt = Convert.ToBase64String(saltBytes);
            PasswordHash = GetHash(password, Salt);
        }
        
        public string PasswordHash { get; private set; }
        
        public string Salt { get; private set; }

        protected static string GetHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(String.Concat(password, salt));
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static bool CheckPassword(string password, string salt, string hash)
        {
            var checkHash = GetHash(password, salt);
            if (checkHash != hash)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
}