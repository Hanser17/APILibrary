using Domain.Enums;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds
{
    public class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            User defaultUser = new()
            {
                UserName = "HanserDextra",
                Email = "Hanser@email.com",
                FirstName = "Hanser",
                LastName = "Pimentel",
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByNameAsync(defaultUser.UserName);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }
    }
}
