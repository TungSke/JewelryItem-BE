using System;
using System.Collections.Generic;

namespace BOs;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public string Category { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal CostPrice { get; set; }

    public decimal Weight { get; set; }

    public bool IsJewelry { get; set; }

    public bool IsGold { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
