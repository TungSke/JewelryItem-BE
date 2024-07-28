using BOs;
using DAOs.Request;
using DAOs.Response;

namespace Repository.Interface
{
    public interface IEmployeeRepo
    {
        Task<Employee> Login(LoginRequest loginRequest);
        Employee GetEmployeeById(int id);
        List<EmployeeResponse> GetAllEmployees(string? search);
        void CreateEmployee(EmployeeRequest employee);
        void DeleteEmployee(int id);
    }
}
