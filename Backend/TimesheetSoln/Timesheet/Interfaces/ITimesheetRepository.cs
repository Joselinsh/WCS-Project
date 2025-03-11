using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheet.Models;

namespace Timesheet.Repositories
{
    public interface ITimesheetRepository
    {
        Task<TimesheetDb> SubmitTimesheet(TimesheetDb timesheet);
        Task<TimesheetDb> GetTimesheetById(int timesheetId);
        Task<List<TimesheetDb>> GetPendingTimesheets(); 
        Task<TimesheetDb> UpdateTimesheet(TimesheetDb timesheet);
        Task<bool> DeleteTimesheet(int timesheetId);
        Task<List<TimesheetDb>> GetTimesheetsByEmployeeId(int employeeId);
    }
}