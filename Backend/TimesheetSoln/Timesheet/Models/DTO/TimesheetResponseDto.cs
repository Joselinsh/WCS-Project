using System;

namespace Timesheet.Models.DTO
{
    public class TimesheetResponseDto
    {
        public int TimesheetId { get; set; }
        public string EmployeeName { get; set; }
        public string ProjectName { get; set; }
        public DateOnly Date { get; set; }
        public int HoursWorked { get; set; }
        public string Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ManagerComments { get; set; } = string.Empty; 
        public string HRComments { get; set; } = string.Empty; 
    }
}