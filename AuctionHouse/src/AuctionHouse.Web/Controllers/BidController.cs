using AuctionHouse.Core.DTOs;
using AuctionHouse.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouse.Web.Controllers;

[Authorize]
public class BidController : Controller
{
    private readonly IBidService bidService;

    public BidController(IBidService bidService)
    {
        this.bidService = bidService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Place(BidViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid bid input.";
            return RedirectToAction("Details", "Auction", new { id = model.AuctionId });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await bidService.PlaceBidAsync(model, userId);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = result.Message;
        }

        return RedirectToAction("Details", "Auction", new { id = model.AuctionId });
    }
}
