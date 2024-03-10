using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
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

        public async Task<bool> AddUserAsync(_User userInput)
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
                var pwdEnc = AesService.Encrypt(user.Id.ToByteArray(), userInput.Password);
                newUser.PasswordHash = pwdEnc;

                return await _context.SaveChangesAsync() > 1;
            }
            return await Task.FromResult(false);
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
                    userPreference = new Preference {
						UserId = currentUser.Id,
						IsDarkMode = isDarkMode
                    };
                    await _context.Preferences.AddAsync(userPreference);
                }

				return await _context.SaveChangesAsync() > 1;
			}
            return await Task.FromResult(false);
		}
    }
}
