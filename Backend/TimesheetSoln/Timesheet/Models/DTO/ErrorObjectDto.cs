﻿namespace Timesheet.Models.DTOs
{
    public class ErrorObjectDto
    {
        public int ErrorNumber { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
