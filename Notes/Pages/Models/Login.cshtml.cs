using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Notes.DAL.Models;

namespace Notes.Pages.Models
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public string Message { get; set; }

        [BindProperty] public new User User { get; set; }

        public LoginModel(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
            Message = "Введите данные";
        }


        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(User.UserName, User.PasswordHash, false, false);
                if (result.Succeeded)
                {
                    Message = "Вы вошли";
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            //RedirectToPage("/Index");
        }
    }
}