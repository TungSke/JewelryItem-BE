using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using DAOs.Request;
using Repository;
using System;

namespace Jewelry_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IConfiguration _configuration;

        public OrdersController(IOrderRepo orderRepo, IConfiguration configuration)
        {
            _orderRepo = orderRepo;
            _configuration=configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            var order = await _orderRepo.CreateOrderAsync(orderRequest);
            await _orderRepo.SendEmail(order.CustomerId);
            return Ok(order);
        }

        [HttpPost("vnpay")]
        public async Task<IActionResult> VnPay(OrderRequest orderRequest)
        {
            string IpAddressRequest = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            orderRequest.PaymentMethod = "VNPAY";
            var order = await _orderRepo.CreateOrderAsync(orderRequest);

            var paymentUrl = _orderRepo.VNPay(double.Parse(order.FinalAmount.ToString()), "thanh toán bằng VNPAY", IpAddressRequest);
                       
            await _orderRepo.SendEmail(orderRequest.CustomerId);
            return Ok(new { Url = paymentUrl });
        }
    }
}
