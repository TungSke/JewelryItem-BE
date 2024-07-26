using BOs;
using DAOs.Request;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class ProductDAO
    {
        private readonly JewelryItemContext _context;

        public ProductDAO(JewelryItemContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts(string? search)
        {
            var list = _context.Products.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) || p.ProductCode.ToLower().Contains(search.ToLower())).ToList();
            }
            return list;
        }

        public Product findbyId(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);
            return product;
        }

        public void addnewProduct(ProductRequest product)
        {
            _context.Products.Add(product.Adapt<Product>());
            _context.SaveChanges();
        }

        public void deleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

    }
}
