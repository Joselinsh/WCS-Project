﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Timesheet.Models.DTO
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        
        public DateOnly? DateOfBirth { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }



        public DateOnly? JoiningDate { get; set; }
    }
}

