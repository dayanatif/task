using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSManager.Data;
using TSManager.Models;

namespace TSManager.Controllers
{
    [Authorize]
    public class SoftwareSubscriptionsController : Controller
    {
        private readonly AppDbContext _db;
        public SoftwareSubscriptionsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.SoftwareSubscriptions
                .OrderBy(s => s.RenewalDate)
                .ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View(new SoftwareSubscription
            {
                SubscribedDate = DateTime.Today,
                RenewalDate = DateTime.Today.AddYears(1)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoftwareSubscription model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.SoftwareSubscriptions.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.SoftwareSubscriptions.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SoftwareSubscription model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.SoftwareSubscriptions.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.SoftwareSubscriptions.FindAsync(id);
            if (item == null) return NotFound();

            _db.SoftwareSubscriptions.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
