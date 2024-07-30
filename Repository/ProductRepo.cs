using BOs;
using DAOs;
using DAOs.Request;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductDAO productDAO;
        public ProductRepo(ProductDAO productDAO)
        {
            this.productDAO = productDAO;
        }

        public void addnewProduct(ProductRequest product) => productDAO.addnewProduct(product);

        public void deleteProduct(int id) => productDAO.deleteProduct(id);
        public void updateProduct(int id, ProductRequest product) => productDAO.UpdateProduct(id, product);

        public Product findbyId(int id) => productDAO.findbyId(id);

        public List<Product> GetAllProducts(string? search) => productDAO.GetAllProducts(search);
    }
}
