using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs;

public class PromotionDAO
{
    private readonly JewelryItemContext _context;

    public PromotionDAO(JewelryItemContext context)
    {
        _context = context;
    }

    public async Task<Promotion> CreatePromotionAsync(Promotion promotion)
    {
        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();
        return promotion;
    }

    public async Task<Promotion?> GetPromotionByIdAsync(int promotionId)
    {
        return await _context.Promotions.FindAsync(promotionId);
    }

    public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        return await _context.Promotions.ToListAsync();
    }

    public async Task<Promotion?> UpdatePromotionAsync(Promotion promotion)
    {
        var existingPromotion = await _context.Promotions.FindAsync(promotion.PromotionId);
        if (existingPromotion != null)
        {
            existingPromotion.PromotionName = promotion.PromotionName;
            existingPromotion.DiscountPercentage = promotion.DiscountPercentage;
            existingPromotion.StartDate = promotion.StartDate;
            existingPromotion.EndDate = promotion.EndDate;
            existingPromotion.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPromotion;
        }
        return null;
    }

    public async Task<bool> DeletePromotionAsync(int promotionId)
    {
        var promotion = await _context.Promotions.FindAsync(promotionId);
        if (promotion != null)
        {
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
