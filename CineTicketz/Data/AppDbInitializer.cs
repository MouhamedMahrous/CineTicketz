using CineTicketz.Data.Static;
using CineTicketz.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CineTicketz.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedUsersAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //seed roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(AppUserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(AppUserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(AppUserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(AppUserRoles.User));

                //seed users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var adminEmail = "admin@CineTicketz.com";

                var admin = await userManager.FindByEmailAsync(adminEmail);

                if(admin is null)
                {
                    var newAdmin = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(newAdmin, AppUserRoles.Admin);
                }


                var appUserEmail = "user@CineTicketz.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);

                if (appUser is null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Appication User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newAppUser, "P@ssw0rd");
                    await userManager.AddToRoleAsync(newAppUser, AppUserRoles.User);
                }
            }
        }
    }
}
