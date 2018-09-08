using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Database;
using Notes.Domain.Models;
using Notes.Domain.Services;

namespace Notes.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("Users/")]
        public IActionResult GetUsers()
        {
            return View("Users", _adminService.GetUsersWithNotes());
        }

        [HttpPost]
        [Route("Users/Save/")]
        public IActionResult SaveUsersToXlsx(string fileName)
        {
            _adminService.SaveUserToXlsx(fileName);
            return RedirectToAction("GetUsers");
        }
    }
}