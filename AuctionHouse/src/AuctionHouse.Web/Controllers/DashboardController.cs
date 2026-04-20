using AuctionHouse.Core.Interfaces;
using AuctionHouse.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IAuctionService auctionService;

    public DashboardController(IAuctionService auctionService)
    {
        this.auctionService = auctionService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserIdOrThrow();
        var model = await auctionService.GetUserDashboardAsync(userId);
        return View(model);
    }
}
