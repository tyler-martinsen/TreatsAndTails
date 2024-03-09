using System;
using System.Collections.Generic;

namespace TreatsAndTails.Models;

public partial class State
{
    public int Id { get; set; }

    public string StateName { get; set; } = null!;

    public string StateAbbreviation { get; set; } = null!;
}
