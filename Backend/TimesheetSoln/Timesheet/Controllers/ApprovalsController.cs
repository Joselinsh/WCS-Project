using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Timesheet.Enum;
using Timesheet.Interfaces;
using Timesheet.Models;
using Timesheet.Models.DTO;
using Timesheet.Repositories;

namespace Timesheet.Controllers
{
    [Route("api/approvals")]
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly ITimesheetService _timesheetService;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHRRepository _HRRepository;

        public ApprovalsController(
            ILeaveService leaveService,
            ITimesheetService timesheetService,
            ILeaveRepository leaveRepository,
            ITimesheetRepository timesheetRepository,
            IEmployeeRepository employeeRepository,
            IHRRepository HRRepository)
        {
            _leaveService = leaveService;
            _timesheetService = timesheetService;
            _leaveRepository = leaveRepository;
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
            _HRRepository = HRRepository;
        }

        [HttpGet("leave/pending")]
        [Authorize]
        public async Task<IActionResult> GetPendingLeaveRequests()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid token. User ID not found.");

            int approverId = int.Parse(userIdClaim);

            var employeeApprover = await _employeeRepository.GetByUserId(approverId);
            var hrApprover = await _HRRepository.GetByUserId(approverId);

            if (employeeApprover == null && hrApprover == null)
                return Unauthorized("You are not authorized to view pending leave requests.");

            if ((employeeApprover?.Designation.Contains("Manager") == true) || (hrApprover != null))
            {
                var pendingLeaves = await _leaveRepository.GetPendingLeaveRequests();
                var filteredLeaves = pendingLeaves
                    .Where(l => hrApprover != null ? l.Status == LeaveStatus.ManagerApproved : l.Status == LeaveStatus.Pending)
                    .Select(l => new
                    {
                        l.Id,
                        l.EmployeeId,
                        EmployeeName = l.Employee?.FullName ?? "Unknown",
                        l.StartDate,
                        l.EndDate,
                        l.Reason,
                        l.ManagerComments,
                        l.HRComments,
                        Status = l.Status.ToString()
                    })
                    .ToList();

                return Ok(filteredLeaves);
            }

            return Unauthorized("You do not have the necessary permissions to view pending leave requests.");
        }

        [HttpGet("timesheet/pending")]
        [Authorize]
        public async Task<IActionResult> GetPendingTimesheets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid token. User ID not found.");

            int approverId = int.Parse(userIdClaim);

            var employeeApprover = await _employeeRepository.GetByUserId(approverId);
            var hrApprover = await _HRRepository.GetByUserId(approverId);

            if (employeeApprover == null && hrApprover == null)
                return Unauthorized("You are not authorized to view pending timesheets.");

            if ((employeeApprover?.Designation.Contains("Manager") == true) || (hrApprover != null))
            {
                var pendingTimesheets = await _timesheetRepository.GetPendingTimesheets();
                var filteredTimesheets = pendingTimesheets
                    .Where(t => hrApprover != null ? t.Status == TimesheetStatus.ManagerApproved : t.Status == TimesheetStatus.Pending)
                    .Select(t => new
                    {
                        t.TimesheetId,
                        t.EmployeeId,
                        EmployeeName = t.Employee?.FullName ?? "Unknown",
                        t.ProjectName,
                        t.Date,
                        t.HoursWorked,
                        t.Description,
                        t.ManagerComments,
                        t.HRComments,
                        Status = t.Status.ToString()
                    })
                    .ToList();

                return Ok(filteredTimesheets);
            }

            return Unauthorized("You do not have the necessary permissions to view pending timesheets.");
        }

        [HttpPost("timesheet-approval")]
        [Authorize]
        public async Task<IActionResult> ApproveTimesheet([FromBody] ApproveTimesheetDto dto)
        {
            Console.WriteLine($"Controller Received - ManagerComments: '{dto.ManagerComments}', HRComments: '{dto.HRComments}'");
            var approverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var employeeApprover = await _employeeRepository.GetByUserId(approverId);
            var hrApprover = await _HRRepository.GetByUserId(approverId);

            if (employeeApprover == null && hrApprover == null)
                return Unauthorized("Approver not found.");

            if ((employeeApprover?.Designation.Contains("Manager") == true) || (hrApprover != null))
            {
                var result = await _timesheetService.ApproveTimesheet(approverId, dto);
                return Ok(new { message = result });
            }

            return Unauthorized("You do not have the necessary permissions to approve timesheets.");
        }

        [HttpPost("leave-approval")]
        [Authorize]
        public async Task<IActionResult> ApproveLeave([FromBody] ApproveLeaveDto dto)
        {
            var approverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var employeeApprover = await _employeeRepository.GetByUserId(approverId);
            var hrApprover = await _HRRepository.GetByUserId(approverId);

            if (employeeApprover == null && hrApprover == null)
                return Unauthorized("Approver not found.");

            if ((employeeApprover?.Designation.Contains("Manager") == true) || (hrApprover != null))
            {
                var result = await _leaveService.ApproveLeave(approverId, dto);
                return Ok(new { message = result });
            }

            return Unauthorized("You do not have the necessary permissions to approve leave requests.");
        }
    }
}