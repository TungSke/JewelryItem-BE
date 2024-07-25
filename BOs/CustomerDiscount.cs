using System;
using System.Collections.Generic;

namespace BOs;

public partial class CustomerDiscount
{
    public int CustomerId { get; set; }

    public int PromotionId { get; set; }

    public decimal DiscountPercentage { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
