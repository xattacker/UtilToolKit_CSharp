using System.Security.Cryptography;
using System.Text;

namespace Xattacker.Utility
{
    // AES 加密類別
    public class AESCipher
    {
        // 加密
        public byte[] Encrypt(byte[] source, string key, string iv)
        {
            byte[] encrypted = null;

            using (RijndaelManaged rijndael = this.CreateRijndael(key, iv))
            {
                ICryptoTransform transform = rijndael.CreateEncryptor();
                encrypted = transform.TransformFinalBlock(source, 0, source.Length);
            }

            return encrypted;
        }

        // 解密
        public byte[] Decrypt(byte[] encrypted, string key, string iv)
        {
            byte[] decrypted = null;

            using (RijndaelManaged rijndael = this.CreateRijndael(key, iv))
            {
                ICryptoTransform transform = rijndael.CreateDecryptor();
                decrypted = transform.TransformFinalBlock(encrypted, 0, encrypted.Length);
            }

            return decrypted;
        }

        private RijndaelManaged CreateRijndael(string key, string iv)
        {
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Key = Encoding.UTF8.GetBytes(key);
            rijndael.IV = Encoding.UTF8.GetBytes(iv);
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;

            return rijndael;
         }
    }
}
