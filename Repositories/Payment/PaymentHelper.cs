using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Payment
{
    public static class PaymentHelper
    {
        public static PaymentResponse GetParamPaymentCallBack(IQueryCollection collection, string hashSecret = null)
        {
            var paymentMethod = "";

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_method"))
                {
                    paymentMethod = value;
                }
            }

            switch (paymentMethod.ToLower())
            {
                case "vnpay":
                    {
                        var vnPay = GetVnPayCallBack(collection, hashSecret);

                        return vnPay;
                    }
            }

            return new PaymentResponse();
        }

        private static PaymentResponse GetVnPayCallBack(IQueryCollection collection, string hashSecret)
        {
            var vnPay = new VnPayLibrary();

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            var orderId = Convert.ToInt64(vnPay.GetResponseData("vnp_TxnRef"));
            var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash =
                collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về
            var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");
            var vnpAmount = vnPay.GetResponseData("vnp_Amount");

            var checkSignature =
                vnPay.ValidateSignature(vnpSecureHash, hashSecret); //check Signature

            if (!checkSignature)
                return new PaymentResponse()
                {
                    Success = false
                };

            var paymentCode = "";

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_code"))
                {
                    paymentCode = value;
                }
            }

            return new PaymentResponse()
            {
                Success = vnpResponseCode.Equals("00"),
                PaymentMethod = "VNPAY",
                BillId = orderInfo,
                OrderId = orderId.ToString(),
                PaymentId = vnPayTranId.ToString(),
                TransactionId = vnPayTranId.ToString(),
                Token = vnpSecureHash,
                PaymentCode = paymentCode,
                TotalAmount = ((int)(double.Parse(vnpAmount) / 100)).ToString()
            };
        }

        public class PaymentResponse
        {
            public string OwnerId { get; set; }
            public string UserId { get; set; }
            public string BillId { get; set; }
            public string TransactionId { get; set; }
            public string OrderId { get; set; }
            public string PaymentMethod { get; set; }
            public string PayerId { get; set; }
            public string PaymentId { get; set; }
            public bool Success { get; set; }
            public string Token { get; set; }
            public string Customer { get; set; }
            public string TotalAmount { get; set; }
            public string PaymentCode { get; set; }

            public string OrderType { get; set; }
        }
    }
}
