using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class VnPayConfig
    {
        public string vnp_TmnCode { get; set; }
        public string vnp_HashSecret { get; set; }
        public string vnp_Url { get; set; }
        public string vnp_ReturnUrl { get; set; }
    }
    public class PaymentRequestDTO
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string IpAddress { get; set; }
    }
}
