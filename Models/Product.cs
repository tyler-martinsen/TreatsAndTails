using System;
using System.Collections.Generic;

namespace TreatsAndTails.Models;

public partial class Product
{
    public int Id { get; set; }

    public string InvShape { get; set; } = null!;

    public string InvFlavor { get; set; } = null!;

    public string InvSize { get; set; } = null!;

    public string InvDescription { get; set; } = null!;

    public decimal Cost { get; set; }

    public int Quantity { get; set; }
}
