using DAOs;
using DAOs.Request;
using DAOs.Response;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Razor.Templating.Core;

namespace Repository;

public class OrderRepo : IOrderRepo 
{
    private readonly OrderDAO _orderDAO;
    private readonly IConfiguration _configuration;
    private readonly IRazorTemplateEngine _razorTemplateEngine;
    

    public OrderRepo(OrderDAO orderDAO, IConfiguration configuration, IRazorTemplateEngine razorTemplateEngine)
    {
        _orderDAO = orderDAO;
        _configuration = configuration;
        _razorTemplateEngine=razorTemplateEngine;
    }
    public Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest) => _orderDAO.CreateOrderAsync(orderRequest);

    public Task<OrderResponse?> GetOrderByIdAsync(int id) => _orderDAO.GetOrderByIdAsync(id);

    public Task<List<OrderResponse>> GetAllOrdersAsync() => _orderDAO.GetAllOrdersAsync();

    public string VNPay(double amount, string orderInfo, string IpAddressRequest)
    {
        try
        {
            const string VNPayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            var vnp_TxnRef = Guid.NewGuid().ToString();
            var vnPayAmount = amount * 100000;

            string baseUrl = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "http://localhost:7000"
            : "https://jewquelry-group4-ewb0dqgndchcc0cm.eastus-01.azurewebsites.net";

            string vnp_HashSecret = "PFWXSZUDDBJVEATFGBOBRLYRXLEBWCCO";

            var requestData = new
            {
                vnp_Version = "2.1.0",
                vnp_Command = "pay",
                vnp_TmnCode = "3F6V0MH8",
                vnp_Amount = vnPayAmount,
                vnp_Locale = "vn",
                vnp_CurrCode = "VND",
                vnp_TxnRef = vnp_TxnRef,
                vnp_OrderInfo = orderInfo,
                vnp_OrderType = "billpayment",
                vnp_ReturnUrl = "https://swp-retake.vercel.app/",
                vnp_IpAddr = IpAddressRequest,
                vnp_CreateDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                vnp_ExpireDate = DateTime.UtcNow.AddMinutes(10).ToString("yyyyMMddHHmmss"), 
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

    public async Task SendEmail(int customerId)
    {
        var latestOrder = await _orderDAO.getLastestOrder(customerId);
        var emailReceive = latestOrder.Customer.Email;
        if (latestOrder == null)
        {
            throw new Exception("No order found for the given customer ID.");
        }

        var emailBody = await _razorTemplateEngine.RenderAsync("EmailTemplate/EmailTemplate.cshtml", latestOrder);
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("trinhsontung24102003@gmail.com", "JewelryItem-Group 4");
            mail.To.Add("trinhsontung2410@gmail.com");
            mail.Subject = "Order Info";
            mail.Body = emailBody ?? "No content available";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            
            smtp.Credentials = new NetworkCredential("trinhsontung24102003@gmail.com", "kebk lwmk jmsl puzl");
            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            throw new Exception("Error when sending email", ex);
        }
    }


}