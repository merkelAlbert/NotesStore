using System.Collections.Generic;
using Notes.Domain.Models;

namespace Notes.Domain.Interfaces
{
    public interface IXlsxService
    {
        void Save(List<UserViewModel> users, string fileName);
    }
}