using System.Collections.Generic;
using Notes.Domain.Models;

namespace Notes.Domain.Interfaces
{
    public interface IAdminService
    {
        List<UserViewModel> GetUsersWithNotes();
        void SaveUserToXlsx(string fileName);
    }
}