using System;
using System.Collections.Generic;

namespace BOs;

public partial class GoldPriceHistory
{
    public int GoldPriceHistoryId { get; set; }

    public decimal GoldPrice { get; set; }

    public DateTime UpdatedAt { get; set; }
}
