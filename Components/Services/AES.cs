
using System;
using System.IO;
using System.Security.Cryptography;

namespace TreatsAndTails.Components.Services
{
    public class AesService
    {
        public static byte[] Encrypt(byte[] key, string original)
        {
            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                // Encrypt the string
                encrypted = EncryptStringToBytes(original, key, aes.IV);
            }
            return encrypted;
        }

        public static string Decrypt(byte[] key, byte[] encrypted)
        {
            string decrypted;

            using (Aes aes = Aes.Create())
            {
                // Encrypt the string
                decrypted = DecryptStringFromBytes(encrypted, key, aes.IV);
            }
            return decrypted;
        }

        #region HelperMethods
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            byte[] encrypted;

            // Create an Aes object with the specified key and IV.
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create a new MemoryStream object to contain the encrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Create a CryptoStream object to perform the encryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Encrypt the plaintext.
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        encrypted = memoryStream.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            string decrypted;

            // Create an Aes object with the specified key and IV.
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create a new MemoryStream object to contain the decrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                {
                    // Create a CryptoStream object to perform the decryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        // Decrypt the ciphertext.
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decrypted = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }
        #endregion
    }
}