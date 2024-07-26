using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BOs;
using Repository.Interface;
using DAOs.Request;

namespace Jewelry_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductsController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
    
        [HttpGet]
        public async Task<IActionResult> GetProducts(string? search)
        {
            return Ok(_productRepo.GetAllProducts(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return Ok(_productRepo.findbyId(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest product)
        {
            _productRepo.addnewProduct(product);
            return Ok("Create success");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _productRepo.deleteProduct(id);
            return Ok("Delete success");
        }
    }
}
