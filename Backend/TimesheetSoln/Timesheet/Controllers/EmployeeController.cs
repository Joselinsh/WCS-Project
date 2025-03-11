using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Timesheet.Interfaces;
using Timesheet.Models.DTO;
using Timesheet.Repositories;

namespace Timesheet.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(
            ITimesheetService timesheetService,
            ITimesheetRepository timesheetRepository,
            ILeaveService leaveService,
            IEmployeeRepository employeeRepository)
        {
            _timesheetService = timesheetService;
            _timesheetRepository = timesheetRepository;
            _leaveService = leaveService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetEmployeeProfile()
        {
            int userId = GetLoggedInUserId();

            var employee = await _employeeRepository.GetByUserIdWithTimesheetsAsync(userId);


            if (employee == null)
                return NotFound("Employee profile not found.");

            return Ok(new
            {
                employee.EmployeeId,
                employee.FullName,
                employee.Email,
                employee.Department,
                employee.Designation,
                Timesheets = employee.Timesheets.Select(t => new
                {
                    t.TimesheetId,
                    t.ProjectName,
                    t.Date,
                    t.HoursWorked,
                    t.Description,
                    t.ManagerComments,
                    t.HRComments,
                    Status = t.Status.ToString()
                }).ToList()
            });
        }


        [HttpGet("timesheets")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeTimesheets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid token. User ID not found.");

            int userId = int.Parse(userIdClaim);
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null)
                return Unauthorized("Employee not found.");

            var timesheets = await _timesheetRepository.GetTimesheetsByEmployeeId(employee.EmployeeId);
            var timesheetDtos = timesheets.Select(t => new TimesheetResponseDto
            {
                TimesheetId = t.TimesheetId,
                EmployeeName = t.Employee?.FullName ?? "Unknown",
                ProjectName = t.ProjectName,
                Date = t.Date,
                HoursWorked = t.HoursWorked,
                Description = t.Description, 
                Status = t.Status.ToString(),
                ManagerComments = t.ManagerComments, 
                HRComments = t.HRComments 
            }).ToList();

            return Ok(timesheetDtos);
        }

        [HttpGet("leaves")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeLeaves()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid token. User ID not found.");

            int userId = int.Parse(userIdClaim);
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null)
                return Unauthorized("Employee not found.");

            var leaves = await _leaveService.GetEmployeeLeaves(employee.EmployeeId);
            return Ok(leaves);
        }

        [HttpPost("Timesheets/Submit")]
        public async Task<IActionResult> SubmitTimesheet([FromBody] SubmitTimesheetDto dto)
        {
            int userId = GetLoggedInUserId();
            var result = await _timesheetService.SubmitTimesheet(dto, userId);
            return Ok(new { message = "Timesheet submitted successfully!", data = result });
        }

        [HttpPut("Timesheets/Update/{timesheetId}")]
        public async Task<IActionResult> UpdateTimesheet(int timesheetId, [FromBody] UpdateTimesheetDto dto)
        {
            int userId = GetLoggedInUserId();
            var result = await _timesheetService.UpdateTimesheet(timesheetId, dto, userId);
            return result != null
                ? Ok(new { message = "Timesheet updated successfully!", data = result })
                : BadRequest("Timesheet cannot be updated because it is already approved or rejected.");
        }

        [HttpDelete("Timesheets/Delete/{timesheetId}")]
        public async Task<IActionResult> DeleteTimesheet(int timesheetId)
        {
            int userId = GetLoggedInUserId();
            bool isDeleted = await _timesheetService.DeleteTimesheet(timesheetId, userId);
            return isDeleted
                ? Ok("Timesheet deleted successfully.")
                : BadRequest("Timesheet cannot be deleted because it is already approved or rejected.");
        }

        [HttpPost("Leaves/Submit")]
        public async Task<IActionResult> SubmitLeave([FromBody] SubmitLeaveDto dto)
        {
            int userId = GetLoggedInUserId();
            var result = await _leaveService.SubmitLeaveRequest(dto, userId);
            return Ok(new { message = "Leave request submitted successfully!", data = result });
        }

        [HttpPut("Leaves/Update/{leaveId}")]
        public async Task<IActionResult> UpdateLeave(int leaveId, [FromBody] UpdateLeaveDto dto)
        {
            int userId = GetLoggedInUserId();
            var result = await _leaveService.UpdateLeave(leaveId, dto, userId);
            return result != null
                ? Ok(new { message = "Leave request updated successfully!", data = result })
                : BadRequest("Leave request cannot be updated because it is already approved or rejected.");
        }


        [HttpDelete("Leaves/Delete/{leaveId}")]
        public async Task<IActionResult> DeleteLeave(int leaveId)
        {
            int userId = GetLoggedInUserId();
            bool isDeleted = await _leaveService.DeleteLeave(leaveId, userId);
            return isDeleted
                ? Ok("Leave request deleted successfully.")
                : BadRequest("Leave request cannot be deleted because it is already approved or rejected.");
        }


        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : throw new UnauthorizedAccessException("User ID not found in token.");
        }
    }
}

    




