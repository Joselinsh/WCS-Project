using Timesheet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

using Timesheet.Models;
namespace Timesheet.Repositories
{
    public interface ILeaveRepository
    {
        Task<LeaveDb> SubmitLeaveRequest(LeaveDb leaveRequest);
        Task<LeaveDb> GetLeaveById(int leaveId);
        Task<List<LeaveDb>> GetPendingLeaveRequests(); 
        
        Task<bool> DeleteLeaveRequest(int leaveId);
        Task<List<LeaveDb>> GetLeavesByEmployeeId(int employeeId);

        Task<LeaveDb> UpdateLeaveRequest(LeaveDb leaveRequest);
    }
}


