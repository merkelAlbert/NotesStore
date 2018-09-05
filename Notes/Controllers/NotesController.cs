﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Database;
using Notes.Domain.Entities;
using Notes.Domain.Models;
using Notes.Domain.Services;

namespace Notes.Controllers
{
    [Route("[controller]")]
    public class NotesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly IdenticonService _identiconService;

        public NotesController(UserManager<IdentityUser> userManager,
            DatabaseContext databaseContext, IdenticonService identiconService)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _identiconService = identiconService;
        }

        [Authorize]
        [Route("create/")]
        public IActionResult CreateNote()
        {
            return View("CreateNote");
        }

        [Authorize]
        [HttpPost]
        [Route("create/")]
        public async Task<IActionResult> CreateNote(NoteViewModel model)
        {
            var note = new Note();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
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

        [Route("delete/")]
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
        [Route("update/")]
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
        [Route("update/")]
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

        [HttpGet]
        [Route("show/")]
        public IActionResult ShowNote(int? id)
        {
            if (id != null)
            {
                var note = _databaseContext.Notes.First(x => x.Id.Equals(id));
                return View("ShowNote", note);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("finded'/")]
        public async Task<IActionResult> FindNote(string searchString)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var notes = _databaseContext.Notes.Where(x =>
                (x.User.Id == user.Id) && (x.Text.ToLower().Contains(searchString.ToLower())
                                           || x.Title.ToLower().Contains(searchString.ToLower())));
            return View("FindedNotes", notes);
        }
    }
}