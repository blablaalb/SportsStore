using Microsoft.AspNetCore.Identity;

namespace SportsStore.Models;

public static class IdentitySeedData
{
    private const string adminUser = "Admin";
    private const string adminPassword = "Secret123$";

    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        context.Database.EnsureCreated();
        UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        //UserManager<IdentityUser> userManager = app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();
        IdentityUser user = await userManager.FindByNameAsync(adminUser);
        if (user == null)
        {
            user = new IdentityUser("Admin");
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}
