using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Notes.Domain.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public int NotesAmount { get; set; }
    }
}