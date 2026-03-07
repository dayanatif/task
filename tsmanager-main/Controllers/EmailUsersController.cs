using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSManager.Data;
using TSManager.Models;

namespace TSManager.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class EmailUsersController : Controller
    {
        private readonly AppDbContext _db;
        public EmailUsersController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            return View(await _db.EmailUsers.ToListAsync());
        }

        public IActionResult Create() => View(new EmailUser());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailUser model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.EmailUsers.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.EmailUsers.FindAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmailUser model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.EmailUsers.FindAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.EmailUsers.FindAsync(id);
            if (item != null)
            {
                _db.EmailUsers.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
