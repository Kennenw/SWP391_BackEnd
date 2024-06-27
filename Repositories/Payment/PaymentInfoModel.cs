using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Payment
{
    public class PaymentInfoModel
    {
        public double TotalAmount { get; set; }
        public string PaymentCode { get; set; }
    }
}
