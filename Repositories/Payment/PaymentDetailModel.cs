﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Payment
{
    public class PaymentDetailModel
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransactionDate { set; get; }
        public decimal Fee { set; get; }
        public Guid StatusId { get; set; }
    }
}
