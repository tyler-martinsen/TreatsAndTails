using System;
using System.Collections.Generic;

namespace TreatsAndTails.Models;

public partial class Location
{
    public Guid? UserId { get; set; }

    public string PhysicalAddressLine1 { get; set; } = null!;

    public string? PhysicalAddressLine2 { get; set; }

    public string PhysicalAddressCity { get; set; } = null!;

    public int PhysicalAddressState { get; set; }

    public string PhysicalAddressZip { get; set; } = null!;

    public virtual State PhysicalAddressStateNavigation { get; set; } = null!;

    public virtual User? User { get; set; }
}
