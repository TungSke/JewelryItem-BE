using DAOs.Request;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace Jewelry_BE.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionRepo _promotionRepo;

    public PromotionsController(IPromotionRepo promotionRepo)
    {
        _promotionRepo = promotionRepo;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] PromotionRequest request)
    {
        var response = await _promotionRepo.CreatePromotionAsync(request);
        return CreatedAtAction(nameof(GetPromotionById), new { id = response.PromotionId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPromotionById(int id)
    {
        var response = await _promotionRepo.GetPromotionByIdAsync(id);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPromotions()
    {
        var responses = await _promotionRepo.GetAllPromotionsAsync();
        return Ok(responses);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePromotion(int id, [FromBody] PromotionRequest request)
    {
        var response = await _promotionRepo.UpdatePromotionAsync(id, request);
        if (response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion(int id)
    {
        var success = await _promotionRepo.DeletePromotionAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}