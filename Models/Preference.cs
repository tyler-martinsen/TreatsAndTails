using System;
using System.Collections.Generic;

namespace TreatsAndTails.Models;

public partial class Preference
{
    public Guid? UserId { get; set; }

    public bool IsDarkMode { get; set; }

    public virtual User? User { get; set; }
}
