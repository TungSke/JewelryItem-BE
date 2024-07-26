using BOs;
using DAOs.Request;
using DAOs.Response;

namespace Repository.Interface
{
    public interface IEmployeeRepo
    {
        public Employee Login(string email, string password);
        Employee GetEmployeeById(int id);
        List<EmployeeResponse> GetAllEmployees(string? search);
        void CreateEmployee(EmployeeRequest employee);
        void DeleteEmployee(int id);
    }
}
