using DAOs;
using DAOs.Request;
using DAOs.Response;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Repository;

public class OrderRepo : IOrderRepo 
{
    private readonly OrderDAO _orderDAO;
    private readonly IConfiguration _configuration;

    public OrderRepo(OrderDAO orderDAO, IConfiguration configuration)
    {
        _orderDAO = orderDAO;
        _configuration = configuration;
    }
    
    public OrderResponse createOrder(OrderRequest orderRequest) => _orderDAO.createOrder(orderRequest);

    public OrderResponse getOrderById(int id) => _orderDAO.getOrderById(id);

    public List<OrderResponse> getAllOrders() => _orderDAO.getAllOrders();

    public string VNPay(double amount, string orderInfo, string IpAddressRequest)
    {
        try
        {
            const string VNPayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            var vnp_TxnRef = Guid.NewGuid().ToString();
            var vnPayAmount = amount * 100000;

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
                vnp_ReturnUrl = "http://localhost:7000/api/Order/returnVnPay", //call api return exist page
                vnp_IpAddr = IpAddressRequest,
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
            return redirectUrl;
        }
        catch (Exception ex)
        {
            throw new Exception("Error when generate VNPay URL", ex);
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