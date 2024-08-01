using System.Text.Json.Serialization;

namespace DAOs.Request;

public class OrderRequest
{
    public string OrderNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;
    [JsonIgnore]
    public string Status { get; set; } = "Done";

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<OrderItemRequest> OrderItems { get; set; } = new List<OrderItemRequest>();
}