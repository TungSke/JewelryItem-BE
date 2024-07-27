namespace DAOs.Response;

public class OrderResponse
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

    public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
}