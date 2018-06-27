using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.DAL.Models;
using Notes.ViewModels;

namespace Notes.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        [BindProperty] public new User User { get; set; }

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        [Route("register/")]
        public IActionResult Register()
        {
            ViewBag.Message = "Регистрация нового пользователя";
            return View("Register");
        }


        [HttpPost]
        [Route("register/")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User {UserName = model.UserName, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToPage("/Index");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var errorMesage = "";
            foreach (var error in errors)
            {
                errorMesage += error.ErrorMessage;
            }

            ViewBag.Message = errorMesage;
            return View("Register");
        }


        [HttpGet]
        [Route("login/")]
        public IActionResult Login()
        {
            ViewBag.Message = "Вход";
            return View("Login");
        }


        [HttpPost]
        [Route("login/")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName,
                        model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Вы вошли";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный email и (или) пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный email и (или) пароль");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var errorMesage = "";
            foreach (var error in errors)
            {
                errorMesage += error.ErrorMessage;
            }

            ViewBag.Message = errorMesage;
            return View("Login");
        }
    }
}