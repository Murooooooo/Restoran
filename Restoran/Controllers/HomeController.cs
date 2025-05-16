using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restoran.DAL;
using Restoran.Models;

namespace Restoran.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var chefs=_context.Chefs.ToList();
        return View(chefs);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
