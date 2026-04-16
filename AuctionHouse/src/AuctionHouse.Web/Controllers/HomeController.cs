using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
