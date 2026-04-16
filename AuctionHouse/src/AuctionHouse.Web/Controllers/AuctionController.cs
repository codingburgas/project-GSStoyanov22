using AuctionHouse.Core.DTOs;
using AuctionHouse.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouse.Web.Controllers;

public class AuctionController : Controller
{
    private readonly IAuctionService auctionService;

    public AuctionController(IAuctionService auctionService)
    {
        this.auctionService = auctionService;
    }

    public async Task<IActionResult> Index()
    {
        var auctions = await auctionService.GetAllAuctionsAsync();
        return View(auctions);
    }

    public async Task<IActionResult> Details(int id)
    {
        var auction = await auctionService.GetAuctionByIdAsync(id);
        if (auction is null)
        {
            return NotFound();
        }

        return View(auction);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View(new AuctionViewModel
        {
            EndTime = DateTime.UtcNow.AddDays(1),
            IsActive = true
        });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuctionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await auctionService.CreateAuctionAsync(model, sellerId);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var auction = await auctionService.GetAuctionByIdAsync(id);
        if (auction is null)
        {
            return NotFound();
        }

        if (!CanManageAuction(auction.SellerId))
        {
            return Forbid();
        }

        return View(auction);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AuctionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isAdmin = User.IsInRole("Admin");
        var updated = await auctionService.UpdateAuctionAsync(model, userId, isAdmin);
        if (!updated)
        {
            return Forbid();
        }

        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var auction = await auctionService.GetAuctionByIdAsync(id);
        if (auction is null)
        {
            return NotFound();
        }

        if (!CanManageAuction(auction.SellerId))
        {
            return Forbid();
        }

        return View(auction);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isAdmin = User.IsInRole("Admin");

        var deleted = await auctionService.DeleteAuctionAsync(id, userId, isAdmin);
        if (!deleted)
        {
            return Forbid();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CanManageAuction(string? sellerId)
    {
        if (User.IsInRole("Admin"))
        {
            return true;
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return !string.IsNullOrWhiteSpace(sellerId) && sellerId == userId;
    }
}
