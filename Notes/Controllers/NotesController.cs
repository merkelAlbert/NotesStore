using System.Linq;
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
    [Authorize]
    [Route("[controller]")]
    public class NotesController : Controller
    {
        private readonly NotesService _notesService;

        public NotesController(NotesService notesService)
        {
            _notesService = notesService;
        }

        [Route("Create/")]
        public IActionResult CreateNote()
        {
            return View("CreateNote");
        }

        [HttpPost]
        [Route("Create/")]
        public async Task<IActionResult> CreateNote(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _notesService.CreateNoteAsync(model, HttpContext);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        [Route("Delete/")]
        public async Task<ActionResult> DeleteNote(int? id)
        {
            if (id != null)
                await _notesService.DeleteNoteAsync((int) id);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("Update/")]
        public IActionResult UpdateNote(int? id)
        {
            if (id != null)
                ViewData["Note"] = _notesService.GetNoteViewModel((int) id);
            return View("CreateNote");
        }

        [HttpPost]
        [Route("Update/")]
        public async Task<IActionResult> UpdateNote(NoteViewModel model, int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null) 
                    await _notesService.UpdateNote(model, (int) id);
                return RedirectToAction("Index", "Home");
            }

            return View("CreateNote");
        }

        [HttpGet]
        [Route("Show/")]
        public IActionResult ShowNote(int? id)
        {
            if (id != null)
            {
                var note = _notesService.GetNote((int) id);
                return View("ShowNote", note);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Finded/")]
        public async Task<IActionResult> FindNotes(string searchString)
        {
            return View("FindedNotes", await _notesService.FindNotes(searchString, HttpContext));
        }
    }
}