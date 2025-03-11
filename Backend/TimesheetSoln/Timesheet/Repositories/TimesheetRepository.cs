using Timesheet.Data;
using Timesheet.Interfaces;
using Timesheet.Models;
using Microsoft.EntityFrameworkCore;
using Timesheet.Models.DTO;
using Timesheet.Enum;

namespace Timesheet.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly TimesheetDbContext _context;

        public TimesheetRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task<TimesheetDb> SubmitTimesheet(TimesheetDb timesheet)
        {
            _context.Timesheets.Add(timesheet);
            await _context.SaveChangesAsync();
            return timesheet;
        }

        public async Task<List<TimesheetDb>> GetEmployeeTimesheets(int employeeId)
        {
            return await _context.Timesheets
                .Where(t => t.EmployeeId == employeeId)
                .Include(t => t.Employee)
                .ToListAsync();
        }

        public async Task<TimesheetDb> GetTimesheetById(int timesheetId)
        {
            return await _context.Timesheets
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.TimesheetId == timesheetId);
        }
        public async Task<TimesheetDb> UpdateTimesheet(TimesheetDb timesheet)
        {
            var existingTimesheet = await _context.Timesheets.FindAsync(timesheet.TimesheetId);
            if (existingTimesheet == null)
                throw new Exception("Timesheet not found");

            Console.WriteLine($"UpdateTimesheet Before - ManagerComments: '{existingTimesheet.ManagerComments}', HRComments: '{existingTimesheet.HRComments}'");
            Console.WriteLine($"UpdateTimesheet Received - ManagerComments: '{timesheet.ManagerComments}', HRComments: '{timesheet.HRComments}'");
            _context.Entry(existingTimesheet).CurrentValues.SetValues(timesheet);
            await _context.SaveChangesAsync();
            Console.WriteLine($"UpdateTimesheet After - ManagerComments: '{existingTimesheet.ManagerComments}', HRComments: '{existingTimesheet.HRComments}'");
            return existingTimesheet;
        }

        public async Task<List<TimesheetDb>> GetTimesheetsByEmployeeId(int employeeId)
        {
            return await _context.Timesheets
                .Where(t => t.EmployeeId == employeeId)
                .Include(t => t.Employee)
                .ToListAsync();
        }

        public async Task<TimesheetDb> UpdateTimesheet(int timesheetId, UpdateTimesheetDto dto)
        {
            var timesheet = await _context.Timesheets.FindAsync(timesheetId);
            if (timesheet == null || timesheet.Status != TimesheetStatus.Pending)
                return null;

            timesheet.ProjectName = dto.ProjectName;
            timesheet.Date = dto.Date;
            timesheet.HoursWorked = dto.HoursWorked;
            timesheet.Description = dto.Description;

            await _context.SaveChangesAsync();
            return timesheet;
        }

        public async Task<bool> DeleteTimesheet(int timesheetId)
        {
            var timesheet = await _context.Timesheets.FindAsync(timesheetId);
            if (timesheet == null || timesheet.Status != TimesheetStatus.Pending)
                return false;

            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TimesheetDb>> GetPendingTimesheets()
        {
            return await _context.Timesheets
                .Include(t => t.Employee)
                .Where(t => t.Status == TimesheetStatus.Pending || t.Status == TimesheetStatus.ManagerApproved)
                .ToListAsync();
        }
    }
}