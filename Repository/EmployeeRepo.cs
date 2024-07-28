using BOs;
using DAOs;
using DAOs.Request;
using DAOs.Response;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly EmployeDAO EmployeDAO;
        public EmployeeRepo(EmployeDAO employeDAO)
        {
            EmployeDAO = employeDAO;
        }

        public void CreateEmployee(EmployeeRequest employee) => EmployeDAO.CreateEmployee(employee);

        public void DeleteEmployee(int id) => EmployeDAO.DeleteEmployee(id);

        public List<EmployeeResponse> GetAllEmployees(string? search) => EmployeDAO.GetAllEmployees(search);

        public async Task<Employee> Login(LoginRequest loginRequest) => await Task.Run(() => EmployeDAO.Login(loginRequest));
        

        public Employee GetEmployeeById(int id) => EmployeDAO.GetEmployeeById(id);

        
    }
}
