using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Database;
using Notes.Domain.Entities;

namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;

        public HomeController(UserManager<IdentityUser> userManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var notes = new List<Note>();
            if (user != null)
            {
                notes = _databaseContext.Notes.Where(note => note.User.Id.Equals(user.Id)).ToList();
            }

            return View("Index", notes);
        }
    }
}