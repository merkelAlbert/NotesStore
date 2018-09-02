using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Notes.Database;
using Notes.Domain.Models;

namespace Notes.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DatabaseContext _databaseContext;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _databaseContext = databaseContext;
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            }
        }


        [HttpGet]
        [Route("register/")]
        public IActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        [Route("register/")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("", "Пользователь с данным email уже существует");
                }
                else
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

                        return RedirectToAction("Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        [Route("login/")]
        public IActionResult Login()
        {
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
                        return RedirectToAction("Index", "Home");
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

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("users/")]
        public IActionResult GetUsers()
        {
            var users = new List<UserViewModel>();
            foreach (var user in _userManager.Users)
            {
                var model = new UserViewModel();
                model.UserName = user.UserName;
                model.NotesAmount = _databaseContext.Notes.Count(note => note.User.Id == user.Id);
                users.Add(model);
            }

            return View("Users", users);
        }

        [HttpGet]
        [Route("logout/")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}