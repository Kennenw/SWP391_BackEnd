using Microsoft.Extensions.Options;
using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Repositories.DTO;

namespace Services
{
    public interface IPaymentServices
    {
        Task<string> CreatePaymentUrl(int bookingId);
        bool ValidateSignature(Dictionary<string, string> queryParams, string signature);
    }

    public class PaymentServices : IPaymentServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly VNPayOptions _vnPayOptions;

        public PaymentServices(UnitOfWork unitOfWork, IOptions<VNPayOptions> vnPayOptions)
        {
            _unitOfWork = unitOfWork;
            _vnPayOptions = vnPayOptions?.Value ?? throw new ArgumentNullException(nameof(vnPayOptions));

            if (string.IsNullOrEmpty(_vnPayOptions.Version))
                throw new ArgumentNullException(nameof(_vnPayOptions.Version));
            if (string.IsNullOrEmpty(_vnPayOptions.TmnCode))
                throw new ArgumentNullException(nameof(_vnPayOptions.TmnCode));
            if (string.IsNullOrEmpty(_vnPayOptions.HashSecret))
                throw new ArgumentNullException(nameof(_vnPayOptions.HashSecret));
            if (string.IsNullOrEmpty(_vnPayOptions.BaseUrl))
                throw new ArgumentNullException(nameof(_vnPayOptions.BaseUrl));
            if (string.IsNullOrEmpty(_vnPayOptions.Command))
                throw new ArgumentNullException(nameof(_vnPayOptions.Command));
            if (string.IsNullOrEmpty(_vnPayOptions.CurrCode))
                throw new ArgumentNullException(nameof(_vnPayOptions.CurrCode));
            if (string.IsNullOrEmpty(_vnPayOptions.Locale))
                throw new ArgumentNullException(nameof(_vnPayOptions.Locale));
            if (string.IsNullOrEmpty(_vnPayOptions.FEUrlCallback))
                throw new ArgumentNullException(nameof(_vnPayOptions.FEUrlCallback));
        }

        public async Task<string> CreatePaymentUrl(int bookingId)
        {
            var booking = _unitOfWork.BookingRepo.GetById(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            var payment = new Payment
            {
                BookingId = bookingId,
                PaymentDate = DateTime.Now,
                PaymentAmount = booking.TotalPrice,
                TotalAmount = booking.TotalPrice,
            };
            _unitOfWork.PaymentRepo.Create(payment);
            await _unitOfWork.SaveAsync();

            var queryParams = new Dictionary<string, string>
            {
                {"vnp_Version", _vnPayOptions.Version },
                {"vnp_Command", _vnPayOptions.Command },
                {"vnp_TmnCode", _vnPayOptions.TmnCode },
                {"vnp_Amount", (booking.TotalPrice * 100).ToString() },
                {"vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                {"vnp_CurrCode", _vnPayOptions.CurrCode },
                {"vnp_IpAddr", "127.0.0.1" },
                {"vnp_Locale", _vnPayOptions.Locale },
                {"vnp_OrderInfo", "Thanh toan cho ma booking: " + bookingId },
                {"vnp_OrderType", "billpayment" },
                {"vnp_ReturnUrl", _vnPayOptions.FEUrlCallback },
                {"vnp_TxnRef", payment.PaymentId.ToString() }
            };

            string secureHash = CreateSignature(queryParams);
            queryParams.Add("vnp_SecureHash", secureHash);

            string paymentUrl = QueryHelpers.AddQueryString(_vnPayOptions.BaseUrl, queryParams);
            return paymentUrl;
        }

        public bool ValidateSignature(Dictionary<string, string> queryParams, string signature)
        {
            string rawData = BuildQueryString(queryParams);
            string computedSignature = ComputeHmacSha512(rawData, _vnPayOptions.HashSecret);
            return computedSignature.Equals(signature, StringComparison.InvariantCultureIgnoreCase);
        }

        private string BuildQueryString(Dictionary<string, string> queryParams)
        {
            var sortedParams = queryParams.OrderBy(kvp => kvp.Key);
            return string.Join("&", sortedParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        }

        private string CreateSignature(Dictionary<string, string> queryParams)
        {
            var sortedParams = queryParams.OrderBy(kvp => kvp.Key);
            string rawData = string.Join("&", sortedParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return ComputeHmacSha512(rawData, _vnPayOptions.HashSecret);
        }

        private string ComputeHmacSha512(string data, string key)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }
    }

    public class VNPayOptions
    {
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string BaseUrl { get; set; }
        public string Command { get; set; }
        public string CurrCode { get; set; }
        public string Version { get; set; }
        public string Locale { get; set; }
        public string TimeZoneId { get; set; }
        public string FEUrlCallback { get; set; }
    }
}
