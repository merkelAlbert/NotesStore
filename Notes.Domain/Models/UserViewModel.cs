using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Notes.Domain.Enums;

namespace Notes.Domain.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int NotesAmount { get; set; }
        public Role Role { get; set; }
    }
}