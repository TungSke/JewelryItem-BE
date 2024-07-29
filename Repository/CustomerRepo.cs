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
    public class CustomerRepo : ICustomerRepo
    {
        private readonly CustomerDAO CustomerDAO;

        public CustomerRepo(CustomerDAO customerDAO)
        {
            CustomerDAO = customerDAO;
        }

        public CustomerResponse createCustomer(CustomerRequest customer) => CustomerDAO.createCustomer(customer);  

        public bool deleteCustomer(int id) => CustomerDAO.deleteCustomer(id);

        public CustomerResponse updateCustomer(int id, CustomerRequest customerRequest) =>
            CustomerDAO.updateCustomer(id, customerRequest);

        public List<CustomerResponse> getAllCustomers(string? search) => CustomerDAO.getAllCustomers(search);

        public CustomerResponse getCustomerById(int id) => CustomerDAO.getCustomerById(id);
    }
}
