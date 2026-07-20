using Domain.Enums;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Seeds
{
    public static class DefaultMCPUser
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            User defaultUser = new()
            {
                UserName = "mcp_agent",
                Email = "mcp_agent@gmail.com",
                FirstName = "MCP",
                LastName = "Agente",
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
                    await userManager.AddToRoleAsync(defaultUser, Roles.MCP.ToString());

                }
            }
        }
    }
}
