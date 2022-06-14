using Microsoft.AspNetCore.Identity;
using Reversi.Models;
using ReversiMvcApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Data
{
    public class ContextSeeder
    {
        public static async Task SeedRolesAsync(UserManager<Speler> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Enums.Rollen.Speler.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Rollen.Mediator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Rollen.Beheerder.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<Speler> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new Speler
            {
                UserName = "collin@admin.com",
                Email = "collin@admin.com",
                Name = "Collin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Polman#11");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Rollen.Beheerder.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Rollen.Speler.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Rollen.Mediator.ToString());
                }

            }
        }
    }
}
