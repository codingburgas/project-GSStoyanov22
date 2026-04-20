using AuctionHouse.Core.Interfaces;
using AuctionHouse.Core.Models;
using AuctionHouse.Infrastructure.Data;
using AuctionHouse.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAuctionService auctionService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ApplicationDbContext dbContext;

    public AdminController(
        IAuctionService auctionService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        this.auctionService = auctionService;
        this.userManager = userManager;
        this.dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var auctions = (await auctionService.GetAllAuctionsAsync()).ToList();
        var users = await userManager.Users
            .OrderBy(u => u.UserName)
            .ToListAsync();

        ViewBag.TotalBids = await dbContext.Bids.CountAsync();
        ViewBag.TotalUsers = users.Count;
        ViewBag.Users = users;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveAuction(int id)
    {
        var userId = User.GetUserIdOrThrow();
        var deleted = await auctionService.DeleteAuctionAsync(id, userId, isAdmin: true);
        TempData[deleted ? "Success" : "Error"] = deleted
            ? "Auction removed successfully."
            : "Failed to remove auction.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleUserBlock(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToAction(nameof(Index));
        }

        user.IsBlocked = !user.IsBlocked;
        await userManager.UpdateAsync(user);

        TempData["Success"] = user.IsBlocked
            ? $"User {user.UserName} is now blocked."
            : $"User {user.UserName} is now unblocked.";

        return RedirectToAction(nameof(Index));
    }
}
