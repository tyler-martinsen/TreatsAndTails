namespace TreatsAndTails.Models
{
	public class _User
	{
		public string Email { get; set; } = null!;

		public string Password { get; set; } = null!;

		public string PasswordConfirmed { get; set; } = null!;

		public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;

		public string? PhoneNumber { get; set; }
	}
}
