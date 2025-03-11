﻿using System;
using Timesheet.Enum;

namespace Timesheet.Models
{
    public class LeaveDb
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        public Employee Employee { get; set; }  
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }

        public string ManagerComments { get; set; } = string.Empty; 
        public string HRComments { get; set; } = string.Empty;
    }
}
