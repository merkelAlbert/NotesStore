using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;


namespace Notes.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var userByEmail = await _userManager.FindByEmailAsync(email);
            if (userByEmail != null)
                return true;
            return false;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new IdentityUser {UserName = model.UserName, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.IsAdmin)
                {
                    var admin = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            return result;
        }

        public async Task<SignInResult> SignInAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = new SignInResult();
            if (user != null)
            {
                result = await _signInManager.PasswordSignInAsync(user.UserName,
                    model.Password, model.RememberMe, false);
            }

            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}