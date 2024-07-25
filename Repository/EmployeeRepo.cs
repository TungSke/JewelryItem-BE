using BOs;
using DAOs;
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
        public Employee Login(string email, string password) => EmployeDAO.Instance.Login(email, password);
    }
}
