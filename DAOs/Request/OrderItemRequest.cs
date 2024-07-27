using System.Text.Json.Serialization;

namespace DAOs.Request;

public class OrderItemRequest
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalPrice { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}