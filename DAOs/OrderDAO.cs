using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOs;
using DAOs.Request;
using DAOs.Response;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DAOs
{
    public class OrderDAO
    {
        private readonly JewelryItemContext _context;

        public OrderDAO(JewelryItemContext context)
        {
            _context = context;
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest)
        {
            var order = orderRequest.Adapt<Order>();
            order.OrderNumber = GenerateUniqueOrderNumber();
            order.OrderItems.Clear();

            foreach (var itemRequest in orderRequest.OrderItems)
            {
                var orderItem = itemRequest.Adapt<OrderItem>();

                // Set UnitPrice from Product and calculate FinalPrice
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == orderItem.ProductId);
                if (product != null)
                {
                    orderItem.UnitPrice = product.UnitPrice;
                    orderItem.FinalPrice = orderItem.UnitPrice * orderItem.Quantity;
                }

                order.OrderItems.Add(orderItem);
            }

            // Calculate the total amount from sum of item final prices
            order.TotalAmount = order.OrderItems.Sum(oi => oi.FinalPrice);

            // Get customer and calculate discount
            var customer = await _context.Customers
                .Include(c => c.CustomerDiscounts)
                .FirstOrDefaultAsync(c => c.CustomerId == order.CustomerId);

            if (customer != null && customer.CustomerDiscounts.Any())
            {
                var maxDiscountPercentage = customer.CustomerDiscounts.Max(cd => cd.DiscountPercentage);
                order.DiscountAmount = (order.TotalAmount * maxDiscountPercentage) / 100;
            }
            else
            {
                order.DiscountAmount = 0;
            }

            // Calculate the final amount
            order.FinalAmount = (decimal)(order.TotalAmount - order.DiscountAmount);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var response = order.Adapt<OrderResponse>();
            response.OrderItems = order.OrderItems.Adapt<List<OrderItemResponse>>();
            return response;
        }


        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return null;
            }

            var response = order.Adapt<OrderResponse>();
            response.OrderItems = order.OrderItems.Adapt<List<OrderItemResponse>>();
            return response;
        }

        public async Task<List<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            var responses = orders.Adapt<List<OrderResponse>>();
            foreach (var response in responses)
            {
                response.OrderItems = orders
                    .First(o => o.OrderId == response.OrderId)
                    .OrderItems.Adapt<List<OrderItemResponse>>();
            }

            return responses;
        }

        private string GenerateUniqueOrderNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var uniquePart = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            return $"ORD-{datePart}-{uniquePart}".Substring(0, 20);
        }

        public async Task<Order?> getLastestOrder(int customerid)
        {
            var order = await _context.Orders.Include(x => x.Customer)
                .Include(o => o.OrderItems).ThenInclude(x => x.Product)
                .Where(o => o.CustomerId == customerid)
                .OrderByDescending(o => o.OrderId)
                .FirstOrDefaultAsync();
            return order;
        }
    }
}
    
