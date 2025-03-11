using Timesheet.Enum;
using Timesheet.Interfaces;
using Timesheet.Models.DTO;
using Timesheet.Models;
using Timesheet.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Timesheet.Service
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHRRepository _HRRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository, IEmployeeRepository employeeRepository, IHRRepository HRRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
            _HRRepository = HRRepository;
        }

        public async Task<TimesheetResponseDto> SubmitTimesheet(SubmitTimesheetDto dto, int userId)
        {
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null) throw new Exception("Employee not found");

            var timesheet = new TimesheetDb
            {
                EmployeeId = employee.EmployeeId,
                ProjectName = dto.ProjectName,
                Date = dto.Date, 
                HoursWorked = dto.HoursWorked,
                Description = dto.Description,
                Status = TimesheetStatus.Pending,
                ManagerComments = string.Empty,
                HRComments = string.Empty
            };

            var savedTimesheet = await _timesheetRepository.SubmitTimesheet(timesheet);
            return new TimesheetResponseDto
            {
                TimesheetId = savedTimesheet.TimesheetId,
                EmployeeName = employee.FullName,
                ProjectName = savedTimesheet.ProjectName,
                Date = savedTimesheet.Date,
                HoursWorked = savedTimesheet.HoursWorked,
                Status = savedTimesheet.Status.ToString(),
                ManagerComments = savedTimesheet.ManagerComments,
                HRComments = savedTimesheet.HRComments,
                Description = savedTimesheet.Description
            };
        }

        public async Task<string> ApproveTimesheet(int approverId, ApproveTimesheetDto dto)
        {
            Console.WriteLine($"Received DTO - ManagerComments: '{dto.ManagerComments}', HRComments: '{dto.HRComments}'");
            var timesheet = await _timesheetRepository.GetTimesheetById(dto.TimesheetId);
            if (timesheet == null) throw new Exception("Timesheet not found");
            Console.WriteLine($"Fetched - ManagerComments: '{timesheet.ManagerComments}', HRComments: '{timesheet.HRComments}'");

            var employeeApprover = await _employeeRepository.GetByUserId(approverId);
            var hrApprover = await _HRRepository.GetByUserId(approverId);
            if (employeeApprover == null && hrApprover == null)
                throw new Exception("Approver not found");

            if (employeeApprover?.Designation.Contains("Manager") == true && timesheet.Status == TimesheetStatus.Pending)
            {
                timesheet.Status = dto.IsApproved ? TimesheetStatus.ManagerApproved : TimesheetStatus.ManagerRejected;
                if (!dto.IsApproved && !string.IsNullOrEmpty(dto.ManagerComments))
                    timesheet.ManagerComments = $"Manager-Rejected: {dto.ManagerComments}";
                else if (!dto.IsApproved && string.IsNullOrEmpty(dto.ManagerComments))
                    timesheet.ManagerComments = "Manager-Rejected: No reason provided";
                else if (dto.IsApproved && !string.IsNullOrEmpty(dto.ManagerComments)) 
                    timesheet.ManagerComments = $"Manager-Approved: {dto.ManagerComments}";
                Console.WriteLine($"Before Update - ManagerComments: '{timesheet.ManagerComments}'");
                await _timesheetRepository.UpdateTimesheet(timesheet);
                Console.WriteLine($"After Update - ManagerComments: '{timesheet.ManagerComments}'");
                return dto.IsApproved ? "Timesheet has been approved by Manager successfully." : "Timesheet has been rejected by Manager.";
            }

            if (hrApprover != null && hrApprover.User.Role == "HR" && timesheet.Status == TimesheetStatus.ManagerApproved)
            {
                timesheet.Status = dto.IsApproved ? TimesheetStatus.Approved : TimesheetStatus.HRRejected;
                if (!dto.IsApproved && !string.IsNullOrEmpty(dto.HRComments))
                    timesheet.HRComments = $"HR-Rejected: {dto.HRComments}";
                else if (!dto.IsApproved && string.IsNullOrEmpty(dto.HRComments))
                    timesheet.HRComments = "HR-Rejected: No reason provided";
                else if (dto.IsApproved && !string.IsNullOrEmpty(dto.HRComments))
                    timesheet.HRComments = $"HR-Approved: {dto.HRComments}";
                Console.WriteLine($"Before Update - HRComments: '{timesheet.HRComments}'");
                await _timesheetRepository.UpdateTimesheet(timesheet);
                Console.WriteLine($"After Update - HRComments: '{timesheet.HRComments}'");
                return dto.IsApproved ? "Timesheet has been approved by HR successfully." : "Timesheet has been rejected by HR.";
            }

            throw new Exception("You are not authorized to approve this timesheet.");
        }

        public async Task<TimesheetResponseDto> UpdateTimesheet(int timesheetId, UpdateTimesheetDto dto, int userId)
        {
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null) throw new Exception("Employee not found");

            var timesheet = await _timesheetRepository.GetTimesheetById(timesheetId);
            if (timesheet == null) throw new Exception("Timesheet not found");

            if (timesheet.EmployeeId != employee.EmployeeId)
                throw new Exception("You can only update your own timesheets.");

            if (timesheet.Status != TimesheetStatus.Pending)
                return null;

            timesheet.ProjectName = dto.ProjectName;
            timesheet.Date = dto.Date; 
            timesheet.HoursWorked = dto.HoursWorked;
            timesheet.Description = dto.Description;

            await _timesheetRepository.UpdateTimesheet(timesheet);

            return new TimesheetResponseDto
            {
                TimesheetId = timesheet.TimesheetId,
                EmployeeName = employee.FullName,
                ProjectName = timesheet.ProjectName,
                Date = timesheet.Date,
                HoursWorked = timesheet.HoursWorked,
                Status = timesheet.Status.ToString(),
                ManagerComments = timesheet.ManagerComments,
                HRComments = timesheet.HRComments,
                Description = timesheet.Description
            };
        }



        public async Task<bool> DeleteTimesheet(int timesheetId, int userId)
        {
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null) throw new Exception("Employee not found");

            var timesheet = await _timesheetRepository.GetTimesheetById(timesheetId);
            if (timesheet == null) return false;

            if (timesheet.EmployeeId != employee.EmployeeId)
                throw new Exception("You can only delete your own timesheets.");

            if (timesheet.Status != TimesheetStatus.Pending)
                return false;

            return await _timesheetRepository.DeleteTimesheet(timesheetId);
        }
    }
}