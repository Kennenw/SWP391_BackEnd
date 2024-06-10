using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPaymentServices
    {
        string CreatePaymentUrl(PaymentRequestDTO paymentRequest, string baseUrl);
        bool VerifySignature(string queryString);
    }
    public class PaymentServices : IPaymentServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly VnPayConfig _config;
        public PaymentServices()
        { }
        public PaymentServices(VnPayConfig config)
        {
            _unitOfWork ??= new UnitOfWork();
            _config = config;
        }
        public string CreatePaymentUrl(PaymentRequestDTO paymentRequest, string baseUrl)
        {
            string locale = "vn";
            string currCode = "VND";
            string vnpVersion = "2.1.0";
            string vnpCommand = "pay";
            string vnpOrderInfo = paymentRequest.OrderDescription;

            SortedDictionary<string, string> vnpParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", vnpVersion },
                { "vnp_Command", vnpCommand },
                { "vnp_TmnCode", _config.vnp_TmnCode },
                { "vnp_Amount", (paymentRequest.Amount * 100).ToString() },
                { "vnp_CurrCode", currCode },
                { "vnp_TxnRef", paymentRequest.OrderId },
                { "vnp_OrderInfo", vnpOrderInfo },
                { "vnp_Locale", locale },
                { "vnp_ReturnUrl", _config.vnp_ReturnUrl },
                { "vnp_IpAddr", paymentRequest.IpAddress },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            string queryString = string.Join("&", vnpParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            string signData = queryString + "&vnp_SecureHash=" + BuildSignature(queryString);
            return _config.vnp_Url + "?" + signData;
        }

        public bool VerifySignature(string queryString)
        {
            string rawQueryString = queryString.Substring(0, queryString.LastIndexOf("&", StringComparison.Ordinal));
            string receivedHash = queryString.Substring(queryString.LastIndexOf("vnp_SecureHash=", StringComparison.Ordinal) + 15);
            string calculatedHash = BuildSignature(rawQueryString);
            return string.Equals(receivedHash, calculatedHash, StringComparison.OrdinalIgnoreCase);
        }

        private string BuildSignature(string inputData)
        {
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_config.vnp_HashSecret));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
