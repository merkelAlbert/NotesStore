using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.Domain.Models;

namespace Notes.Domain.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetUsersWithNotesAsync();
        Task ChangeUserRoleAsync(string userId, string role);
        Task SaveUserToXlsxAsync(string fileName);
    }
}