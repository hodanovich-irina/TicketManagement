using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TicketManagement.UserAPI.Dto;

namespace TicketManagement.UserAPI.Initializers
{
    /// <summary>
    /// Class initializer.
    /// </summary>
    public class RoleInitializer
    {
        protected RoleInitializer()
        {
        }

        /// <summary>
        /// Method for initialize start initial roles.
        /// </summary>
        /// <param name="userManager">user manager.</param>
        /// <param name="roleManager">role manager.</param>
        /// <param name="configuration">configuration.</param>
        /// <returns>Task.</returns>
        public static async Task InitializeAsync(UserManager<UserDto> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            string adminEmail = configuration["AdminLogin"];
            string password = configuration["AdminPassword"];
            if (await roleManager.FindByNameAsync(Role.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
            }

            if (await roleManager.FindByNameAsync(Role.User) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.User));
            }

            if (await roleManager.FindByNameAsync(Role.VenueManager) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.VenueManager));
            }

            if (await roleManager.FindByNameAsync(Role.EventManager) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Role.EventManager));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new UserDto { Email = adminEmail, UserName = adminEmail, Surname = adminEmail, TimeZoneId = TimeZoneInfo.Local.Id, Patronymic = adminEmail, Name = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Role.Admin);
                }
            }
        }
    }
}
