using System;
using System.Collections.Generic;

namespace BOs;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public string Status { get; set; } = "Not Yet";

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
