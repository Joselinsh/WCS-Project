using System;

namespace Timesheet.Models.DTO
{
    public class LeaveResponseDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }
        public string ManagerComments { get; set; } = string.Empty; 
        public string HRComments { get; set; } = string.Empty; 
    }
}