using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Database;
using Notes.Domain.Entities;
using Notes.Domain.Interfaces;

namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotesService _notesService;

        public HomeController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View("Index", await _notesService.GetNotes(HttpContext));
        }
    }
}