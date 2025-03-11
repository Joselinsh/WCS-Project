using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Data;
using Timesheet.Enum;
using Timesheet.Interfaces;
using Timesheet.Models;
using Timesheet.Repositories;

namespace Timesheet.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly TimesheetDbContext _context;

        public LeaveRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveDb> SubmitLeaveRequest(LeaveDb leave)
        {
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return leave;
        }

        public async Task<LeaveDb> GetLeaveById(int leaveId)
        {
            return await _context.Leaves
                .Include(l => l.Employee) 
                .FirstOrDefaultAsync(l => l.Id == leaveId);
        }

        public async Task<List<LeaveDb>> GetLeavesByEmployeeId(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .Include(l => l.Employee) 
                .ToListAsync();
        }

        public async Task<LeaveDb> UpdateLeaveRequest(LeaveDb leave)
        {
            var existingLeave = await _context.Leaves.FindAsync(leave.Id);
            if (existingLeave == null || existingLeave.Status != LeaveStatus.Pending)
                return null;

            existingLeave.StartDate = leave.StartDate;
            existingLeave.EndDate = leave.EndDate;
            existingLeave.Reason = leave.Reason;
            existingLeave.ManagerComments = leave.ManagerComments; 
            existingLeave.HRComments = leave.HRComments; 

            _context.Leaves.Update(existingLeave);
            await _context.SaveChangesAsync();
            return existingLeave; 
        }

        public async Task<bool> DeleteLeaveRequest(int leaveId)
        {
            var leave = await _context.Leaves.FindAsync(leaveId);
            if (leave == null || leave.Status != LeaveStatus.Pending)
                return false; 

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<LeaveDb>> GetPendingLeaveRequests()
        {
            return await _context.Leaves
                .Include(l => l.Employee) 
                .Where(l => l.Status == LeaveStatus.Pending || l.Status == LeaveStatus.ManagerApproved)
                .ToListAsync();
        }
    }
}