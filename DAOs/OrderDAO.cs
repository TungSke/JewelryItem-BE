using BOs;
using DAOs.Request;
using DAOs.Response;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DAOs;

public class OrderDAO
{
    private readonly JewelryItemContext _context;

    public OrderDAO(JewelryItemContext context)
    {
        _context = context;
    }

    public OrderResponse createOrder(OrderRequest orderRequest)
    {
        var order = orderRequest.Adapt<Order>();
        order.OrderNumber = GenerateUniqueOrderNumber();
        order.OrderItems.Clear();

        foreach (var itemRequest in orderRequest.OrderItems)
        {
            var orderItem = itemRequest.Adapt<OrderItem>();

            // Set UnitPrice from Product and calculate FinalPrice
            var product = _context.Products.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
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
        var customer = _context.Customers
            .Include(c => c.CustomerDiscounts)
            .FirstOrDefault(c => c.CustomerId == order.CustomerId);

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
        _context.SaveChanges();

        var response = order.Adapt<OrderResponse>();
        response.OrderItems = order.OrderItems.Adapt<List<OrderItemResponse>>();
        return response;
    }

    public OrderResponse getOrderById(int id)
    {
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.OrderId == id);

        if (order == null)
        {
            return null;
        }

        var response = order.Adapt<OrderResponse>();
        response.OrderItems = order.OrderItems.Adapt<List<OrderItemResponse>>();
        return response;
    }

    public List<OrderResponse> getAllOrders()
    {
        var orders = _context.Orders
            .Include(o => o.OrderItems)
            .ToList();

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
}