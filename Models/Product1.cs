using System;
using System.Collections.Generic;

namespace TreatsAndTails.Models;

public partial class Product1
{
    public Guid? UserId { get; set; }

    public int Ordered { get; set; }

    public int Shipped { get; set; }

    public int Delivered { get; set; }

    public virtual User? User { get; set; }
}
