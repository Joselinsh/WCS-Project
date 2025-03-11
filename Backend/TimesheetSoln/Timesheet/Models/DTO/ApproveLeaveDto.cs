namespace Timesheet.Models.DTO
{
    public class ApproveLeaveDto
    {
        public int LeaveId { get; set; }

        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
