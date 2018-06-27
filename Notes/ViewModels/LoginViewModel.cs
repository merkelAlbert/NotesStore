﻿using System.ComponentModel.DataAnnotations;

namespace Notes.ViewModels
{
    public class LoginViewModel
    {
        [Required] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        

        public bool RememberMe { get; set; }
    }
}