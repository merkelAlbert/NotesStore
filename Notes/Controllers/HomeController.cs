using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.DAL;
using Notes.DAL.Models;

namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DatabaseContext _databaseContext;

        public HomeController(Microsoft.AspNetCore.Identity.UserManager<User> userManager,
            SignInManager<User> signInManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _databaseContext = databaseContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                var notes = _databaseContext.Notes.Where(note => note.User.Id.Equals(user.Id)).ToList();
                foreach (var note in notes)
                {
                    Console.WriteLine(note.Text);
                }
                return View("Index", notes);
            }

            return View();
        }
    }
}