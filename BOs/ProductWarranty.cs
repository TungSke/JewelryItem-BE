using System;
using System.Collections.Generic;

namespace BOs;

public partial class ProductWarranty
{
    public int WarrantyId { get; set; }

    public int OrderItemId { get; set; }

    public int WarrantyPeriod { get; set; }

    public DateTime WarrantyExpireDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual OrderItem OrderItem { get; set; } = null!;
}
