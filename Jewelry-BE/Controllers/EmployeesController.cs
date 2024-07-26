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

        [HttpGet("login")]
        public async Task<IActionResult> Login(string email,string password)
        {
            var employee = _employeeRepo.Login(email, password);
            return Ok(employee);
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
    }
}
