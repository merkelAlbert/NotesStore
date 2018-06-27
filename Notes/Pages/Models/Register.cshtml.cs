using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Notes.DAL;
using Notes.DAL.Models;

namespace Notes.Pages.Models
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public string Message { get; set; }

        [BindProperty] public new User User { get; set; }

        public RegisterModel(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public void OnGet()
        {
            Message = "Введите данные";
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(User, User.PasswordHash);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(User, false);
                    return RedirectToPage("/Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return RedirectToPage("/Index");
        }
    }
}