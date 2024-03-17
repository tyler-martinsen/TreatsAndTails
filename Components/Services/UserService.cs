using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Cryptography;
using System.Text;
using TreatsAndTails.Models;

namespace TreatsAndTails.Components.Services
{
	public class UserService
	{
		private readonly TatContext _context;

		public UserService(TatContext context)
		{
			this._context = context;
		}

		#region Users

		public async Task<bool> AddUserAsync(_User userInput)
		{
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{
				try
				{
					User user = new User
					{
						Email = userInput.Email,
						PasswordHash = new byte[] { 0 },
						FirstName = userInput.FirstName,
						LastName = userInput.LastName,
						PhoneNumber = userInput.PhoneNumber,
						IsAdmin = false,
						RegistrationDate = DateTime.UtcNow,
					};

					await _context.Users.AddAsync(user);
					await _context.SaveChangesAsync();

					User? newUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == userInput.Email);

					if (newUser != null)
					{
						var encKey = (await _context.Users.FirstOrDefaultAsync(user => user.Email == userInput.Email))?.Id.ToByteArray();
						if (encKey != null)
						{
							var pwdEnc = EncryptStringToBytes(userInput.Password, encKey);
							newUser.PasswordHash = pwdEnc;

							var changesSaved = await _context.SaveChangesAsync();

							dbContextTransaction.Commit();

							return changesSaved > 0;
						}
						return await Task.FromResult(false);
					}
					return await Task.FromResult(false);
				}
				catch (Exception ex)
				{
					return await Task.FromResult(false);
				}
			}
		}

		public async Task<bool> SetDarkmodeAsync(string? email, bool isDarkMode)
		{
			if (email == null)
			{
				return await Task.FromResult(false);
			}
			User? currentUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
			if (currentUser != null)
			{
				Preference? userPreference = await _context.Preferences.FirstOrDefaultAsync(user => user.Equals(currentUser));

				if (userPreference != null)
				{
					userPreference.IsDarkMode = isDarkMode;
				}
				else
				{
					userPreference = new Preference
					{
						UserId = currentUser.Id,
						IsDarkMode = isDarkMode
					};
					await _context.Preferences.AddAsync(userPreference);
				}

				return await _context.SaveChangesAsync() > 1;
			}
			return await Task.FromResult(false);
		}
		#endregion

		#region AES

		public static async Task<byte[]?> GetKey(string username)
		{
			using (var db = new TatContext())
			{
				var encKey = (await db.Users.FirstOrDefaultAsync(x => x.Email == username))?.Id.ToByteArray();
				return encKey;
			}
		}

		public async Task<bool> AuthenticateUser(string username, string password)
		{
			var encryptionKey = (await _context.Users.FirstOrDefaultAsync(user => user.Email == username))?.Id.ToByteArray();
			if (encryptionKey != null)
			{
				var storedPassword = (await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(username)))?.PasswordHash;

				if (storedPassword != null && storedPassword.Length > 0)
				{
					var decryptedPassword = DecryptStringFromBytes(storedPassword, encryptionKey);

					return password == decryptedPassword;
				}
			}
			return await Task.FromResult(false);
		}

		static byte[] EncryptStringToBytes(string plainText, byte[] key)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.GenerateIV(); // Generate a random IV for encryption

				// Encrypt the string to an array of bytes
				byte[] encrypted = aes.IV;
				using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
				{
					byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
					encrypted = encrypted.Concat(encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length)).ToArray();
				}

				return encrypted;
			}
		}

		static string? DecryptStringFromBytes(byte[] cipherText, byte[] key)
		{
			try
			{
				using (Aes aes = Aes.Create())
				{
					aes.Key = key;

					// Extract IV from the beginning of the cipherText
					byte[] iv = new byte[aes.IV.Length];
					Array.Copy(cipherText, 0, iv, 0, iv.Length);
					aes.IV = iv;

					// Decrypt the bytes to a string
					using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
					{
						byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, iv.Length, cipherText.Length - iv.Length);
						return Encoding.UTF8.GetString(decryptedBytes);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
	}
	#endregion
}
