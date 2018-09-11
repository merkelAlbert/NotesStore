using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Notes.Domain.Enums;

namespace Notes.Domain.Utils
{
    public class RolesInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesInitializer(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Role.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
                var admin = new IdentityUser
                {
                    Email = "admin@admin.ru",
                    UserName = "admin",
                };
                var password = "admin1";
                var result = await _userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, Role.Admin.ToString());
            }

            if (!await _roleManager.RoleExistsAsync(Role.User.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.User.ToString()));
            }
        }
    }
}