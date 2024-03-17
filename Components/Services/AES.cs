using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TreatsAndTails.Models;

namespace TreatsAndTails.Components.Services
{
    public class AesService
    {
		private readonly TatContext _context;

		public AesService(TatContext context)
		{
			this._context = context;
		}

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

        public async Task<byte[]?> GetKey(string username)
        {
			return (await _context.Users.FirstOrDefaultAsync(x => x.Email == username))?.Id.ToByteArray();
		}

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var encryptionKey = await GetKey(username);
            if (encryptionKey == null)
            {
                return await Task.FromResult(false);
            }

            var encryptedPassword = Encrypt(encryptionKey, password);

            if(encryptedPassword == null || encryptedPassword.Length == 0)
            {
				return await Task.FromResult(false);
			}

            var storedPassword = (await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(username)))?.PasswordHash;

			if (storedPassword == null || storedPassword.Length == 0)
			{
				return await Task.FromResult(false);
			}

            return storedPassword == encryptedPassword;
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