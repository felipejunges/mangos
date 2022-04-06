using System;
using System.Security.Cryptography;
using System.Text;

namespace Mangos.Dominio.Utils
{
    public class Cripto
    {
        #region MD5

        public static string MD5Encrypt(string toEncrypt)
        {
            string md5String;
            byte[] encStringBytes;
            UTF8Encoding encoder = new UTF8Encoding();

            var md5 = MD5.Create();

            encStringBytes = encoder.GetBytes(toEncrypt);

            encStringBytes = md5.ComputeHash(encStringBytes);

            md5String = BitConverter.ToString(encStringBytes);
            md5String = md5String.Replace("-", "");

            return md5String.ToLower();
        }

        public static bool MD5Verify(string toVerify, string hash)
        {
            return (MD5Encrypt(toVerify) == hash);
        }

        #endregion

        #region SHA1

        public static string SHA1Encrypt(string toEncrypt)
        {
            string sha1String;
            byte[] encStringBytes;
            UTF8Encoding encoder = new UTF8Encoding();

            var sha512 = SHA512.Create();

            encStringBytes = encoder.GetBytes(toEncrypt);

            encStringBytes = sha512.ComputeHash(encStringBytes);

            sha1String = BitConverter.ToString(encStringBytes);
            sha1String = sha1String.Replace("-", "");

            return sha1String.ToLower();
        }

        public static bool SHA1Verify(string toVerify, string hash)
        {
            return SHA1Encrypt(toVerify) == hash;
        }

        #endregion

        #region AES

        public static string AESDecrypt(string toDecrypt, string chave)
        {
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
            byte[] keyArray = GenerateKey(chave, 128);

            var aes = Aes.Create();
            aes.Key = keyArray;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] decrypted = decryptor.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            aes.Clear();

            return System.Text.UTF8Encoding.UTF8.GetString(decrypted);
        }

        public static string AESEncrypt(string toEncrypt, string chave)
        {
            byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] keyArray = GenerateKey(chave, 128);

            var aes = Aes.Create();
            aes.Key = keyArray;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateEncryptor();

            byte[] encrypted = decryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            aes.Clear();

            return Convert.ToBase64String(encrypted);
        }

        private static byte[] GenerateKey(string chave, int bits)
        {
            byte[] keyArray = Convert.FromBase64String(MD5Encrypt(chave));
            byte[] keyBytes = new byte[bits / 8];

            int len = keyArray.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;

            Array.Copy(keyArray, keyBytes, len);

            return keyBytes;
        }

        #endregion

        #region BCrypt

        public static string BCryptGenerate(string senha, int salt = 12)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha, BCrypt.Net.BCrypt.GenerateSalt(salt));
        }

        public static bool BCryptCheck(string senha, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(senha, hash);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}