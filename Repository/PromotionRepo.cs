using BOs;
using DAOs;
using DAOs.Request;
using DAOs.Response;
using Repository.Interface;

namespace Repository;

public class PromotionRepo : IPromotionRepo
{
    private readonly PromotionDAO _promotionDAO;

    public PromotionRepo(PromotionDAO promotionDAO)
    {
        _promotionDAO = promotionDAO;
    }
    
    public async Task<PromotionResponse> CreatePromotionAsync(PromotionRequest request)
    {
        var promotion = new Promotion
        {
            PromotionName = request.PromotionName,
            DiscountPercentage = request.DiscountPercentage,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var createdPromotion = await _promotionDAO.CreatePromotionAsync(promotion);
        return new PromotionResponse
        {
            PromotionId = createdPromotion.PromotionId,
            PromotionName = createdPromotion.PromotionName,
            DiscountPercentage = createdPromotion.DiscountPercentage,
            StartDate = createdPromotion.StartDate,
            EndDate = createdPromotion.EndDate
        };
    }

    public async Task<PromotionResponse?> GetPromotionByIdAsync(int promotionId)
    {
        var promotion = await _promotionDAO.GetPromotionByIdAsync(promotionId);
        if (promotion != null)
        {
            return new PromotionResponse
            {
                PromotionId = promotion.PromotionId,
                PromotionName = promotion.PromotionName,
                DiscountPercentage = promotion.DiscountPercentage,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate
            };
        }
        return null;
    }

    public async Task<IEnumerable<PromotionResponse>> GetAllPromotionsAsync()
    {
        var promotions = await _promotionDAO.GetAllPromotionsAsync();
        return promotions.Select(p => new PromotionResponse
        {
            PromotionId = p.PromotionId,
            PromotionName = p.PromotionName,
            DiscountPercentage = p.DiscountPercentage,
            StartDate = p.StartDate,
            EndDate = p.EndDate
        });
    }

    public async Task<PromotionResponse?> UpdatePromotionAsync(int promotionId, PromotionRequest request)
    {
        var promotion = new Promotion
        {
            PromotionId = promotionId,
            PromotionName = request.PromotionName,
            DiscountPercentage = request.DiscountPercentage,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            UpdatedAt = DateTime.UtcNow
        };
        var updatedPromotion = await _promotionDAO.UpdatePromotionAsync(promotion);
        if (updatedPromotion != null)
        {
            return new PromotionResponse
            {
                PromotionId = updatedPromotion.PromotionId,
                PromotionName = updatedPromotion.PromotionName,
                DiscountPercentage = updatedPromotion.DiscountPercentage,
                StartDate = updatedPromotion.StartDate,
                EndDate = updatedPromotion.EndDate
            };
        }
        return null;
    }

    public async Task<bool> DeletePromotionAsync(int promotionId)
    {
        return await _promotionDAO.DeletePromotionAsync(promotionId);
    }
}