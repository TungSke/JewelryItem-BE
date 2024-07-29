using BOs;
using DAOs.Request;
using DAOs.Response;
using Mapster;

namespace DAOs
{
    public class CustomerDAO
    {
        private readonly JewelryItemContext _context;

        public CustomerDAO(JewelryItemContext context)
        {
            _context = context;
        }

        public List<CustomerResponse> getAllCustomers(string? search)
        {
            var list = _context.Customers.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(c => c.FullName.Contains(search)).ToList();
            }
            var mapper = list.Adapt<List<CustomerResponse>>();
            
            return mapper;
        }

        public CustomerResponse getCustomerById(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return null;
            }
            var mapper = customer.Adapt<CustomerResponse>();
            return mapper;
        }

        public CustomerResponse createCustomer(CustomerRequest customer)
        {
            _context.Customers.Add(customer.Adapt<Customer>());
            _context.SaveChanges();
            var mapper = customer.Adapt<CustomerResponse>();
            return mapper;
        }

        public bool deleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return false;
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return true;
        }

        public CustomerResponse updateCustomer(int id, CustomerRequest customerRequest)
        {
            var existingCustomer = _context.Customers.Find(id);
            if (existingCustomer == null)
            {
                return null;
            }
            existingCustomer.FullName = customerRequest.FullName;
            existingCustomer.Email = customerRequest.Email;
            existingCustomer.PhoneNumber = customerRequest.PhoneNumber;
            _context.SaveChanges();
            var updatedCustomerResponse = existingCustomer.Adapt<CustomerResponse>();
            return updatedCustomerResponse;
        }
    }
}
