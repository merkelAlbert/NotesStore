using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.DAL;
using Notes.DAL.Models;
using Notes.ViewModels;

namespace Notes.Controllers
{
    public class NotesController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DatabaseContext _databaseContext;

        public NotesController(Microsoft.AspNetCore.Identity.UserManager<User> userManager,
            SignInManager<User> signInManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _databaseContext = databaseContext;
        }

        [Authorize]
        [Route("createNote/")]
        public IActionResult CreateNote()
        {
            return View("CreateNote");
        }

        [Authorize]
        [HttpPost]
        [Route("createNote/")]
        public async Task<IActionResult> CreateNote(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    var note = new Note();
                    note.Text = model.Text;
                    note.Title = model.Title;
                    note.User = user;
                    await _databaseContext.Notes.AddAsync(note);
                    _databaseContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }
    }
}