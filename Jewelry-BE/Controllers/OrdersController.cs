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

        public OrdersController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }
        
        [HttpGet("VNPay")]
        public IActionResult VNPay(double amount, string orderInfo)
        {
            try
            {
                const string VNPayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                var vnp_TxnRef = Guid.NewGuid().ToString();
                var vnPayAmount = amount * 100000; // Số tiền cần thanh toán theo đơn vị của VNPay
                string clientIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

                // Mã bí mật của bạn từ cấu hình
                string vnp_HashSecret = "PFWXSZUDDBJVEATFGBOBRLYRXLEBWCCO";

                var requestData = new
                {
                    vnp_Version = "2.1.0",
                    vnp_Command = "pay",
                    vnp_TmnCode = "3F6V0MH8",
                    vnp_Amount = vnPayAmount,
                    vnp_Locale = "vn",
                    vnp_CurrCode = "VND",
                    vnp_TxnRef = vnp_TxnRef, //số hóa đơn (dùng trong database) nên dùng GUID để tránh trùng lặp
                    vnp_OrderInfo = orderInfo, //nội dung thanh toán (description)   
                    vnp_OrderType = "billpayment",
                    vnp_ReturnUrl = "http://localhost:5090/api/Accounts/returnVnPay", //call api return exist page
                    vnp_IpAddr = clientIpAddress,
                    vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    vnp_ExpireDate = DateTime.Now.AddMinutes(2).ToString("yyyyMMddHHmmss"),
                };

                // Sắp xếp các thuộc tính của yêu cầu theo thứ tự chữ cái
                var sortedData = requestData.GetType().GetProperties()
                    .OrderBy(p => p.Name)
                    .ToDictionary(p => p.Name, p => p.GetValue(requestData)?.ToString() ?? "");

                // Tạo query string từ dữ liệu đã sắp xếp
                var queryString = string.Join("&", sortedData.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

                // Tạo chữ ký cho yêu cầu
                var signature = GenerateVnPaySignature(queryString, vnp_HashSecret);

                // Thêm chữ ký vào dữ liệu yêu cầu
                sortedData.Add("vnp_SecureHashType", "SHA512");
                sortedData.Add("vnp_SecureHash", signature);

                // Tạo query string mới từ dữ liệu đã có chữ ký
                var queryStringWithSignature = string.Join("&", sortedData.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

                // Tạo URL hoàn chỉnh cho VNPay
                var redirectUrl = $"{VNPayUrl}?{queryStringWithSignature}";
                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string GenerateVnPaySignature(string data, string hashSecret)
        {
            // Chuyển mã bí mật và dữ liệu cần ký thành mảng byte
            byte[] hashSecretBytes = Encoding.UTF8.GetBytes(hashSecret);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            // Sử dụng HMAC-SHA512 để tạo chữ ký
            using (var hmac = new HMACSHA512(hashSecretBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(dataBytes);
                // Chuyển mảng byte thành chuỗi hex
                return string.Concat(hashBytes.Select(b => b.ToString("x2")));
            }
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
            return Ok(order);
        }
    }
}
