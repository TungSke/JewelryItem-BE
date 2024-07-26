using BOs;
using DAOs.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace Jewelry_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomersController(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet]
        public IActionResult getAllCustomers([FromQuery] string? search)
        {
            return Ok(_customerRepo.getAllCustomers(search));
        }

        [HttpGet("{id}")]
        public IActionResult getCustomerById(int id)
        {
            return Ok(_customerRepo.getCustomerById(id));
        }

        [HttpPost]
        public IActionResult createCustomer([FromBody] CustomerRequest customer)
        {
            return Ok(_customerRepo.createCustomer(customer));
        }

        [HttpDelete("{id}")]
        public IActionResult deleteCustomer(int id)
        {
            return Ok(_customerRepo.deleteCustomer(id));
        }
    }
}
