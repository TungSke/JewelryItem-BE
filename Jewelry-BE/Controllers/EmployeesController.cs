using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BOs;
using Repository.Interface;

namespace Jewelry_BE.Controllers
{
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
    }
}
