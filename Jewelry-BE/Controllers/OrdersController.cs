using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using DAOs.Request;
using Repository;

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
        public IActionResult getAllOrders()
        {
            return Ok(_orderRepo.getAllOrders());
        }

        [HttpGet("{id}")]
        public IActionResult getOrderById(int id)
        {
            return Ok(_orderRepo.getOrderById(id));
        }

        [HttpPost]
        public IActionResult createOrder([FromBody] OrderRequest orderRequest)
        {
            return Ok(_orderRepo.createOrder(orderRequest));
        }

        [HttpPost("vnpay")]
        public IActionResult VnPay(OrderRequest orderRequest)
        {
            string IpAddressRequest = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var paymentUrl = _orderRepo.VNPay(double.Parse(orderRequest.FinalAmount.ToString()), "thanh toán bằng VNPAY", IpAddressRequest);
            orderRequest.PaymentMethod = "VNPAY";
            _orderRepo.createOrder(orderRequest);
            return Ok(new { Url = paymentUrl });
        }

        [HttpGet("returnVnPay")]
        public IActionResult ReturnVnPay(string url)
        {
            try
            {
                // Lấy các tham số từ query string
                var queryParams = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
                Console.WriteLine(queryParams);

                // Lấy mã bí mật từ cấu hình
                string vnp_HashSecret = _configuration["VNPay:HashSecret"];

                // Tạo lại query string từ các tham số, ngoại trừ vnp_SecureHash và vnp_SecureHashType
                var orderedParams = queryParams
                    .Where(kvp => kvp.Key != "vnp_SecureHash" && kvp.Key != "vnp_SecureHashType")
                    .OrderBy(kvp => kvp.Key)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var rawData = string.Join("&", orderedParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

                // Tạo chữ ký từ dữ liệu
                var receivedSignature = queryParams["vnp_SecureHash"];
                var calculatedSignature = GenerateVnPaySignature(rawData, vnp_HashSecret);

                // Xác thực chữ ký
                if (calculatedSignature == receivedSignature)
                {
                    // Lấy trạng thái thanh toán
                    var paymentStatus = queryParams["vnp_ResponseCode"];

                    if (paymentStatus == "00")
                    {
                        
                        return Redirect("https://destinymatch.vercel.app/");
                    }
                    else
                    {
                        // Thanh toán thất bại
                        return BadRequest("Payment failed");
                    }
                }
                else
                {
                    // Chữ ký không hợp lệ
                    return BadRequest("Invalid signature");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, "Internal server error");
            }
        }

        private string GenerateVnPaySignature(string rawData, string hashSecret)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(System.Text.Encoding.UTF8.GetBytes(hashSecret)))
            {
                byte[] hashValue = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }
    }
}
