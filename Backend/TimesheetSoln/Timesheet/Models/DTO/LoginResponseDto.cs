﻿namespace Timesheet.Models.DTO
{
    public class LoginResponseDto
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public int? RoleId { get; set; }
        public string Designation { get; set; }
     
    }
}
