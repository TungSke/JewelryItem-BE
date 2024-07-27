using DAOs.Request;
using DAOs.Response;

namespace Repository.Interface;

public interface IPromotionRepo
{
    Task<PromotionResponse> CreatePromotionAsync(PromotionRequest request);
    Task<PromotionResponse?> GetPromotionByIdAsync(int promotionId);
    Task<IEnumerable<PromotionResponse>> GetAllPromotionsAsync();
    Task<PromotionResponse?> UpdatePromotionAsync(int promotionId, PromotionRequest request);
    Task<bool> DeletePromotionAsync(int promotionId);
}