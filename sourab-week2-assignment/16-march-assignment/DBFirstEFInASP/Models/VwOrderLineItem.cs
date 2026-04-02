using System;
using System.Collections.Generic;

namespace DBFirstEFInASP.Models;

public partial class VwOrderLineItem
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string CustomerName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public short Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }
}
