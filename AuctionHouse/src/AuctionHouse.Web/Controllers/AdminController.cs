using AuctionHouse.Core.Interfaces;
using AuctionHouse.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAuctionService auctionService;

    public AdminController(IAuctionService auctionService)
    {
        this.auctionService = auctionService;
    }

    public async Task<IActionResult> Index()
    {
        var auctions = await auctionService.GetAllAuctionsAsync();
        return View(auctions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EndAuction(int id)
    {
        var userId = User.GetUserIdOrThrow();
        var ended = await auctionService.EndAuctionAsync(id, userId, isAdmin: true);
        TempData[ended ? "Success" : "Error"] = ended
            ? "Auction ended successfully."
            : "Failed to end auction.";

        return RedirectToAction(nameof(Index));
    }
}
