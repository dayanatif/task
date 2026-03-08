using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSManager.Data;
using TSManager.Models;

namespace TSManager.Controllers
{
    [Authorize]
    public class DomainsController : Controller
    {
        private readonly AppDbContext _db;
        public DomainsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.Domains
                .OrderBy(d => d.RenewalDate)
                .ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View(new DomainDetail
            {
                RegisteredDate = DateTime.Today,
                RenewalDate = DateTime.Today.AddYears(1),
                AutoRenew = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DomainDetail model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Domains.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Domains.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DomainDetail model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Domains.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Domains.FindAsync(id);
            if (item == null) return NotFound();

            _db.Domains.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
