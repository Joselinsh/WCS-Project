using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Timesheet.Data;
using Timesheet.Interfaces;
using Timesheet.Models;
using Timesheet.Models.DTO;


namespace Timesheet.Services
{
    public class RoleService : IRoleService
    {
        private readonly TimesheetDbContext _context;

        public RoleService(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user == null) return false; 

            if (user.Role != "Unassigned" ) return false; 

            user.Role = dto.Role;
            await _context.SaveChangesAsync();

           
            if (dto.Role == "Employee")
            {
                var employee = new Employee
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Department = user.Department,
                    Designation = user.Designation 
                };
                _context.Employees.Add(employee);
            }
            else if (dto.Role == "HR")
            {
                var hr = new HR
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Department = user.Department,
                    Designation = user.Designation 
                };
                _context.HRs.Add(hr);
            }
            else if (dto.Role == "Admin") 
            {
                var admin = new Admins
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Department = user.Department,
                   
                };
                _context.Admins.Add(admin);
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}

