using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSManager.ViewModels;
using TSManager.Services;
using AspNetCoreGeneratedDocument;


namespace TSManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailService _email;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, EmailService email)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _email = email;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> CreateUser()
        {
            // Ensure roles exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            return View(new CreateUserViewModel());
        }

        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RunExpiryNow([FromServices] TSManager.Services.ExpiryNotificationRunner runner)
        {
            await runner.RunAsync(HttpContext.RequestAborted);
            TempData["Msg"] = "Expiry job executed now.";
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Ensure roles exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            return RedirectToAction(nameof(Users));
        }

    }
}
