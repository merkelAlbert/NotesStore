using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Database;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;
using Notes.Domain.Services;

namespace Notes.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
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