namespace Timesheet.Models.DTO
{
    public class ApproveTimesheetDto
    {
        public int TimesheetId { get; set; }
        public bool IsApproved { get; set; }
        public string ManagerComments { get; set; }
        public string HRComments { get; set; }
    }
}
