using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheet.Interfaces;
using Timesheet.Models;
using Timesheet.Models.DTO;
using Timesheet.Enum;
using Timesheet.Repositories; // Added for ILeaveRepository

namespace Timesheet.Service
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHRRepository _HRRepository;

        public LeaveService(ILeaveRepository leaveRepository, IEmployeeRepository employeeRepository, IHRRepository HRRepository)
        {
            _leaveRepository = leaveRepository;
            _employeeRepository = employeeRepository;
            _HRRepository = HRRepository;
        }

        public async Task<LeaveResponseDto> SubmitLeaveRequest(SubmitLeaveDto dto, int userId)
        {
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null)
                throw new Exception("Employee not found");

            if (dto.StartDate < DateOnly.FromDateTime(DateTime.UtcNow)) // Fixed comparison
                throw new Exception("Leave request cannot be for past dates.");

            var leaveRequest = new LeaveDb
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.FullName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = LeaveStatus.Pending,
                ManagerComments = string.Empty,
                HRComments = string.Empty
            };

            var savedLeave = await _leaveRepository.SubmitLeaveRequest(leaveRequest);
            return new LeaveResponseDto
            {
                Id = savedLeave.Id,
                EmployeeName = savedLeave.EmployeeName,
                StartDate = savedLeave.StartDate,
                EndDate = savedLeave.EndDate,
                Status = savedLeave.Status.ToString(),
                ManagerComments = savedLeave.ManagerComments,
                HRComments = savedLeave.HRComments
            };
        }

        public async Task<string> ApproveLeave(int approverId, ApproveLeaveDto dto)
        {
            var leaveRequest = await _leaveRepository.GetLeaveById(dto.LeaveId);
            if (leaveRequest == null)
                throw new Exception("Leave request not found");

            var employeeApprover = await _employeeRepository.GetByUserId(approverId);
            var hrApprover = await _HRRepository.GetByUserId(approverId);

            if (employeeApprover == null && hrApprover == null)
                throw new Exception("Approver not found");

            if (employeeApprover?.Designation.Contains("Manager") == true && leaveRequest.Status == LeaveStatus.Pending)
            {
                leaveRequest.Status = dto.IsApproved ? LeaveStatus.ManagerApproved : LeaveStatus.ManagerRejected;
                if (!dto.IsApproved && !string.IsNullOrEmpty(dto.Comments))
                    leaveRequest.ManagerComments = $"Manager-Rejected: {dto.Comments}";
                else if (!dto.IsApproved && string.IsNullOrEmpty(dto.Comments))
                    leaveRequest.ManagerComments = "Manager-Rejected: No reason provided";
                await _leaveRepository.UpdateLeaveRequest(leaveRequest);
                return dto.IsApproved ? "Leave request has been approved by Manager." : "Leave request has been rejected by Manager.";
            }

            if (hrApprover != null && hrApprover.User?.Role == "HR" && leaveRequest.Status == LeaveStatus.ManagerApproved)
            {
                leaveRequest.Status = dto.IsApproved ? LeaveStatus.Approved : LeaveStatus.HRRejected;
                if (!dto.IsApproved && !string.IsNullOrEmpty(dto.Comments))
                    leaveRequest.HRComments = $"HR-Rejected: {dto.Comments}";
                else if (!dto.IsApproved && string.IsNullOrEmpty(dto.Comments))
                    leaveRequest.HRComments = "HR-Rejected: No reason provided";
                await _leaveRepository.UpdateLeaveRequest(leaveRequest);
                return dto.IsApproved ? "Leave request has been approved by HR." : "Leave request has been rejected by HR.";
            }

            throw new Exception("You are not authorized to approve this leave request.");
        }

        public async Task<List<LeaveResponseDto>> GetEmployeeLeaves(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid Employee ID");

            var leaves = await _leaveRepository.GetLeavesByEmployeeId(employeeId);

            if (leaves == null || leaves.Count == 0)
                return new List<LeaveResponseDto>();

            var leaveDtos = new List<LeaveResponseDto>();
            foreach (var leave in leaves)
            {
                leaveDtos.Add(new LeaveResponseDto
                {
                    Id = leave.Id,
                    EmployeeName = leave.Employee?.FullName ?? "Unknown",
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Status = leave.Status.ToString(),
                    ManagerComments = leave.ManagerComments,
                    HRComments = leave.HRComments
                });
            }

            return leaveDtos;
        }

        public async Task<LeaveResponseDto> UpdateLeave(int leaveId, UpdateLeaveDto dto, int userId)
        {
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null) throw new Exception("Employee not found");

            var leave = await _leaveRepository.GetLeaveById(leaveId);
            if (leave == null) throw new Exception("Leave request not found");

            if (leave.EmployeeId != employee.EmployeeId)
                throw new Exception("You can only update your own leave requests.");

            if (leave.Status != LeaveStatus.Pending)
                return null;

            leave.StartDate = dto.StartDate;
            leave.EndDate = dto.EndDate;
            leave.Reason = dto.Reason;

            await _leaveRepository.UpdateLeaveRequest(leave);

            return new LeaveResponseDto
            {
                Id = leave.Id,
                EmployeeName = employee.FullName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Status = leave.Status.ToString(),
                ManagerComments = leave.ManagerComments,
                HRComments = leave.HRComments
            };
        }

        public async Task<bool> DeleteLeave(int leaveId, int userId)
        {
            var employee = await _employeeRepository.GetByUserId(userId);
            if (employee == null) throw new Exception("Employee not found");

            var leave = await _leaveRepository.GetLeaveById(leaveId);
            if (leave == null) return false;

            if (leave.EmployeeId != employee.EmployeeId)
                throw new Exception("You can only delete your own leave requests.");

            if (leave.Status != LeaveStatus.Pending)
                return false;

            return await _leaveRepository.DeleteLeaveRequest(leaveId);
        }
    }
}