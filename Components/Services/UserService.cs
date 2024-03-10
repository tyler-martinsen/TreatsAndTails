using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddUser(_User userInput)
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
    }
}
