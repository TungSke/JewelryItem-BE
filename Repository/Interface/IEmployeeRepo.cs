using BOs;

namespace Repository.Interface
{
    public interface IEmployeeRepo
    {
        public Employee Login(string email, string password);
    }
}
