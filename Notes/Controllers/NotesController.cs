using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Notes.DAL;
using Notes.DAL.Models;
using Notes.Services;
using Notes.ViewModels;

namespace Notes.Controllers
{
    public class NotesController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DatabaseContext _databaseContext;
        private readonly IdenticonService _identiconService;

        public NotesController(Microsoft.AspNetCore.Identity.UserManager<User> userManager,
            SignInManager<User> signInManager, DatabaseContext databaseContext, IdenticonService identiconService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _databaseContext = databaseContext;
            _identiconService = identiconService;
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
            var note = new Note();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    note.Text = model.Text;
                    note.Title = model.Title;
                    note.User = user;
                    note.Identicon =
                        _identiconService.GetIdenticon((model.Text + model.Title).GetHashCode().ToString());
                    await _databaseContext.Notes.AddAsync(note);
                    _databaseContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        [Route("deleteNote/")]
        public async Task<ActionResult> DeleteNote(int? id)
        {
            if (id != null)
            {
                var note = _databaseContext.Notes.First(x => x.Id.Equals(id));
                if (note != null)
                {
                    _databaseContext.Notes.Remove(note);
                    await _databaseContext.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("updateNote/")]
        public IActionResult UpdateNote(int? id)
        {
            if (id != null)
            {
                var note = _databaseContext.Notes.First(x => x.Id.Equals(id));
                ViewData["note"] = note;
            }

            return View("CreateNote");
        }

        [HttpPost]
        [Route("updateNote/")]
        public async Task<IActionResult> UpdateNote(NoteViewModel model, int? id)
        {
            if (ModelState.IsValid)
            {
                var note = _databaseContext.Notes.First(x => x.Id.Equals(id));
                note.Text = model.Text;
                note.Title = model.Title;
                note.Identicon =
                    _identiconService.GetIdenticon((model.Text + model.Title).GetHashCode().ToString());
                _databaseContext.Update(note);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View("CreateNote");
        }
    }
}