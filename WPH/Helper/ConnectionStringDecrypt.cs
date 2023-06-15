using System;
using System.Security.Cryptography;
using System.Text;

namespace WPH.Helper
{
    public static class ConnectionStringDecrypt
    {
        public static string Decrypt(string encryptedText)
        {
            try
            {
                string key = "B9SupmNM1gV/0nyli1qS3o5akkzFhO+CoqByEb0yU9A=";
                string IV = "dCsOgZ3hl3pe7RlRrH6Aig==";

                Aes cipher = CreateCipher(key);
                cipher.IV = Convert.FromBase64String(IV);

                ICryptoTransform cryptTransform = cipher.CreateDecryptor();
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                var result = Encoding.UTF8.GetString(plainBytes);
                return result;
            }
            catch
            {
                return "";
            }
        }

        private static Aes CreateCipher(string keyBase64)
        {
            Aes cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;

            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);

            return cipher;
        }
    }
}
