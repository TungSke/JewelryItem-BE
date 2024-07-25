using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class EmployeDAO
    {
        private readonly JewelryItemContext _context;
        private static EmployeDAO instance = null;
        public static EmployeDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmployeDAO();
                }
                return instance;
            }
        }

        private EmployeDAO()
        {
            _context = new JewelryItemContext();
        }

        public Employee Login(string email, string password)
        {
            return _context.Employees.FirstOrDefault(e => e.Email == email && e.Password == password);
        }
    }
}
