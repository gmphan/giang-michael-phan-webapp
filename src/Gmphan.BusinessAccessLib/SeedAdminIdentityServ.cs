using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gmphan.BusinessAccessLib
{
    public class SeedAdminIdentityServ : ISeedAdminIdentityServ
    {
        private readonly ILogger<SeedAdminIdentityServ> _logger;
        private readonly InitAdminUser _initAdminUser;
        public SeedAdminIdentityServ(ILogger<SeedAdminIdentityServ> logger
                                    , IOptions<InitAdminUser> options)
        {
            _logger = logger;
            _initAdminUser = options.Value;
        }
        public async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            // Get the UserManager and RoleManager services
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure that the Admin role exists
            if (!await roleManager.RoleExistsAsync(_initAdminUser.AdminRole))
            {
                _logger.LogDebug("here");
                await roleManager.CreateAsync(new IdentityRole(_initAdminUser.AdminRole));
            }

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(_initAdminUser.AdminEmail);
            if (adminUser == null)
            {
                // Create the admin user if they don't exist
                adminUser = new IdentityUser
                {
                    UserName = _initAdminUser.AdminEmail,
                    Email = _initAdminUser.AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, _initAdminUser.AdminPassword);

                // If the user is created successfully, assign the Admin role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, _initAdminUser.AdminRole);
                }
            }
        }
    }
}