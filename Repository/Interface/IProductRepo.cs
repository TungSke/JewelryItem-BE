using BOs;
using DAOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IProductRepo
    {
        List<Product> GetAllProducts(string? search);
        Product findbyId(int id);
        void addnewProduct(ProductRequest product);
        void deleteProduct(int id);
    }
}
