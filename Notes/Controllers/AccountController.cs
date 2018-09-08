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
using Notes.Domain.Services;

namespace Notes.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("Register/")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [Route("Register/")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.UserExistsAsync(model.Email))

                    ModelState.AddModelError("", "Пользователь с данным email уже существует");
                else
                {
                    var result = await _accountService.RegisterAsync(model);
                    if (result.Succeeded)
                        return RedirectToAction("Login");
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("Login/")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("Login/")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.SignInAsync(model);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", "Неправильный email и (или) пароль");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("Logout/")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}