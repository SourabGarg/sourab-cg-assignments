using System;
using System.Collections.Generic;

namespace DBFirstEFInASP.Models;

public partial class VwCustomerOrderView
{
    public string CustomerId { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? City { get; set; }

    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? Freight { get; set; }
}
