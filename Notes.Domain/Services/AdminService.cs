using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Notes.Database;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;

namespace Notes.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly IXlsxService _xlsxService;

        public AdminService(UserManager<IdentityUser> userManager, DatabaseContext databaseContext, IXlsxService xlsxService)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _xlsxService = xlsxService;
        }

        public List<UserViewModel> GetUsersWithNotes()
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

        public void SaveUserToXlsx(string fileName)
        {
            var users = GetUsersWithNotes();
            if (string.IsNullOrEmpty(fileName))
                fileName = "Users";
            _xlsxService.Save(users, fileName);
        }
    }
}