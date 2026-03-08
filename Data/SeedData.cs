using Microsoft.AspNetCore.Identity;

namespace TSManager.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services, string adminEmail)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
                await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
