﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Timesheet.Interfaces;
using Timesheet.Models;
using Timesheet.Models.DTO;
using Timesheet.Repositories;

namespace Timesheet.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHRRepository _HRRepository;
        private readonly IAdminRepository _adminRepository;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IEmployeeRepository employeeRepository, IHRRepository hRRepository, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _HRRepository = hRRepository;
            _adminRepository = adminRepository;
        }

        public async Task<UserRegistrationResponseDto> RegisterAsync(UserRegistrationDto userregistrationDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userregistrationDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userregistrationDto.Password));
            var passwordSalt = hmac.Key;

            var user = new User
            {
                FullName = userregistrationDto.FullName,
                Email = userregistrationDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = userregistrationDto.PhoneNumber,
                DateOfBirth = userregistrationDto.DateOfBirth,
                Department = userregistrationDto.Department,
                JoiningDate = userregistrationDto.JoiningDate,
                Designation=userregistrationDto.Designation,
                Role = "Unassigned" // Default role
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return new UserRegistrationResponseDto
            {
                
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Department = user.Department,
                JoiningDate = user.JoiningDate,
                Designation=user.Designation,
                Message = $"Welcome {user.FullName}, you have registered successfully!"
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
                throw new Exception("User not found.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                throw new Exception("Invalid password.");

         
            if (user.Role == "Unassigned")
            {
                return new LoginResponseDto
                {
                    Message = "Login successful, but role is not assigned. Please contact Admin.",
                    Token = null,
                    RoleId=user.Id
                };
            }

           
            var secretKey = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("JWT Secret Key is missing in configuration.");

            var key = Encoding.UTF8.GetBytes(secretKey);

           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                  
                new Claim(ClaimTypes.Role, user.Role),
               
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);


            int? roleId = null;
            string designation = null;
            if (user.Role == "Employee")
            {
                var employee = await _employeeRepository.GetEmployeeByUserIdAsync(user.Id);
                roleId = employee?.EmployeeId;
                designation = employee?.Designation; 
            }


            else if (user.Role == "HR")
            {
                var hr = await _HRRepository.GetHRByUserIdAsync(user.Id);
                roleId = hr?.Id ?? 0; 
                designation = hr?.Designation;
            }

            else if (user.Role == "Admin") 
            {
                var admin = await _adminRepository.GetAdminByUserIdAsync(user.Id);
                roleId = admin?.Id ?? 0;
                designation = "Admin";
            }
            
            string welcomeMessage = user.Role switch
            {
                "Admin" => "Welcome to Admin Dashboard",
                "HR" => "Welcome to HR Dashboard",
                "Employee" => designation?.Contains("Manager") == true ? "Welcome to Manager Dashboard" : "Welcome to Employee Dashboard",
                _ => "Unauthorized access"
            };

            return new LoginResponseDto
            {
                Message = welcomeMessage,
                Token = jwtToken,
                RoleId=roleId,
                Designation = designation
            };
        }
    }
}
