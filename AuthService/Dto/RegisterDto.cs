﻿using System.ComponentModel.DataAnnotations;

namespace AuthService.Controllers
{
    public class RegisterDto
    {
        [Required]
        [StringLength(64, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]       
        public string Username { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]       
        public string Password { get; set; }
    }
}