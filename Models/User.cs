using System;
using System.Collections.Generic;

namespace TreatsAndTails.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public bool IsAdmin { get; set; }

    public DateTime RegistrationDate { get; set; }
}
