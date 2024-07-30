using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BOs;
using Repository.Interface;
using DAOs.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jewelry_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeesController(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request");
            }

            var employee = await _employeeRepo.Login(loginRequest);

            if (employee == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var jwtToken = GenerateToken(employee.EmployeeId.ToString(), employee.Email, employee.Role, employee.FullName);
            return Ok(new { token = jwtToken });
        }

        private string GenerateToken(string id, string email, string role, string? name)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authenticationthis is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            name = name ?? "";
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Name, name),
            };

            var token = new JwtSecurityToken(
               "localhost:7777",
                "localhost:7777",
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(string? searchString)
        {
            var employees = _employeeRepo.GetAllEmployees(searchString);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = _employeeRepo.GetEmployeeById(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRequest employee)
        {
            _employeeRepo.CreateEmployee(employee);
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            _employeeRepo.DeleteEmployee(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeRequest request)
        {
            _employeeRepo.UpdateEmployee(id, request);
            return Ok("Update Success");
        }
    }
}
