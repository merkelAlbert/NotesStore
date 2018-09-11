using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IAccountService _accountService;


        public HomeController(INotesService notesService, IAccountService accountService)
        {
            _notesService = notesService;
            _accountService = accountService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View("Index", _notesService.GetNotes(await _accountService.GetCurrentUserIdAsync(HttpContext)));
        }
    }
}