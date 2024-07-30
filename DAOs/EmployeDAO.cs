using BOs;
using DAOs.Request;
using DAOs.Response;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
    public class EmployeDAO
    {
        private readonly JewelryItemContext _context;
        
        public EmployeDAO(JewelryItemContext context)
        {
            _context = context;
        }

        public async Task<Employee?> Login(LoginRequest loginRequest)
        {
            return await _context.Employees
                .Where(e => e.Email == loginRequest.Email && e.Password == loginRequest.Password)
                .FirstOrDefaultAsync();
        }

        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.Find(id);
        }

        public List<EmployeeResponse> GetAllEmployees(string? search)
        {
            var list = _context.Employees.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(e => e.FullName.ToLower().Contains(search.ToLower()) || e.Email.ToLower().Contains(search.ToLower())).ToList();
            }
            var mapperlist = list.Adapt<List<EmployeeResponse>>();
            return mapperlist;
        }

        public void CreateEmployee(EmployeeRequest employee)
        {
            _context.Employees.Add(employee.Adapt<Employee>());
            _context.SaveChanges();
        }

        public void DeleteEmployee(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
            }
        }

        public void UpdateEmployee(int id, EmployeeRequest request)
        {
            var exist = _context.Employees.Find(id);
            if (exist != null)
            {
                exist.FullName = request.FullName;
                exist.PhoneNumber = request.PhoneNumber;
                exist.Email = request.Email;
                exist.Password = request.Password;
                exist.Role = request.Role;
                exist.Department = request.Department;
                _context.SaveChanges();
            }
        }


    }
}
