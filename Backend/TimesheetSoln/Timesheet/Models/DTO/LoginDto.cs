﻿using System.ComponentModel.DataAnnotations;

namespace Timesheet.Models.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
