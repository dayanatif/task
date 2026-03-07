using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TSManager.Data;
using TSManager.Models;
using TSManager.ViewModels;

namespace TSManager.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var d30 = today.AddDays(30);
        var d60 = today.AddDays(60);

        var model = new DashboardViewModel
        {
            Days30 = 30,
            Days60 = 60,

            SoftwareExpiring30 = await _db.SoftwareSubscriptions
                .Where(x => x.RenewalDate >= today && x.RenewalDate <= d30)
                .OrderBy(x => x.RenewalDate)
                .ToListAsync(),

            SoftwareExpiring60 = await _db.SoftwareSubscriptions
                .Where(x => x.RenewalDate > d30 && x.RenewalDate <= d60)
                .OrderBy(x => x.RenewalDate)
                .ToListAsync(),

            DomainsExpiring30 = await _db.Domains
                .Where(x => x.RenewalDate >= today && x.RenewalDate <= d30)
                .OrderBy(x => x.RenewalDate)
                .ToListAsync(),

            DomainsExpiring60 = await _db.Domains
                .Where(x => x.RenewalDate > d30 && x.RenewalDate <= d60)
                .OrderBy(x => x.RenewalDate)
                .ToListAsync()
        };

        return View(model);
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
