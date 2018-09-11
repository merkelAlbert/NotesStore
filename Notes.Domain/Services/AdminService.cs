using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Notes.Database;
using Notes.Domain.Enums;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;

namespace Notes.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly IXlsxService _xlsxService;

        public AdminService(UserManager<IdentityUser> userManager, DatabaseContext databaseContext,
            IXlsxService xlsxService)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _xlsxService = xlsxService;
        }

        public async Task<List<UserViewModel>> GetUsersWithNotesAsync()
        {
            var users = new List<UserViewModel>();
            foreach (var user in _userManager.Users)
            {
                var model = new UserViewModel();
                model.UserName = user.UserName;
                model.UserId = user.Id;
                model.Role = (Role) Enum.Parse(typeof(Role), (await _userManager.GetRolesAsync(user))[0]);
                model.NotesAmount = _databaseContext.Notes.Count(note => note.User.Id == user.Id);
                users.Add(model);
            }

            return users;
        }

        public async Task ChangeUserRoleAsync(string userId, string role)
        {
            var roles = Enum.GetNames(typeof(Role));
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task SaveUserToXlsxAsync(string fileName)
        {
            var users = await GetUsersWithNotesAsync();
            if (string.IsNullOrEmpty(fileName))
                fileName = "Users";
            _xlsxService.Save(users, fileName);
        }
    }
}