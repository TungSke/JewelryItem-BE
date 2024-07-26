using BOs;
using DAOs.Request;
using DAOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICustomerRepo
    {
        List<CustomerResponse> getAllCustomers(string? search);
        CustomerResponse getCustomerById(int id);
        CustomerResponse createCustomer(CustomerRequest customer);
        bool deleteCustomer(int id);
    }
}
