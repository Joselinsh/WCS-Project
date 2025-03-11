using Timesheet.Models.DTO;
using System.Threading.Tasks;

namespace Timesheet.Interfaces
{
    public interface ITimesheetService
    {
        Task<TimesheetResponseDto> SubmitTimesheet(SubmitTimesheetDto dto, int userId);

        Task<string> ApproveTimesheet(int approverId, ApproveTimesheetDto dto);

        Task<TimesheetResponseDto> UpdateTimesheet(int timesheetId, UpdateTimesheetDto dto, int userId);

        Task<bool> DeleteTimesheet(int timesheetId, int userId);
    }
}
