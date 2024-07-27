namespace DAOs.Request;

public class PromotionRequest
{
    public string PromotionName { get; set; } = null!;
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}