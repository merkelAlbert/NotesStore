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
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly XlsxService _xlsxService;

        public UsersController(UserManager<IdentityUser> userManager, DatabaseContext databaseContext,
            XlsxService xlsxService)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _xlsxService = xlsxService;
        }


        private List<UserViewModel> GetUserViewModels()
        {
            var users = new List<UserViewModel>();
            foreach (var user in _userManager.Users)
            {
                var model = new UserViewModel();
                model.UserName = user.UserName;
                model.NotesAmount = _databaseContext.Notes.Count(note => note.User.Id == user.Id);
                users.Add(model);
            }

            return users;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return View("Users", GetUserViewModels());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("save/")]
        public IActionResult SaveUsersToXlsx(string fileName)
        {
            var users = GetUserViewModels();
            if (string.IsNullOrEmpty(fileName))
                fileName = "Users";
            _xlsxService.Save(users, fileName);
            return RedirectToAction("GetUsers");
        }
    }
}