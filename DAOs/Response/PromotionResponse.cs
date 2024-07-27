namespace DAOs.Response;

public class PromotionResponse
{
    public int PromotionId { get; set; }
    public string PromotionName { get; set; } = null!;
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}