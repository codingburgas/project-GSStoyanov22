using AuctionHouse.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly IAuctionService auctionService;

    public ReportsController(IAuctionService auctionService)
    {
        this.auctionService = auctionService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await auctionService.GetReportsAsync();
        return View(model);
    }
}
